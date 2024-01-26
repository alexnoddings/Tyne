using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Tyne.Aerospace.Client.Features.Data.PartTypes;

public static class EditPartType
{
    public class Request : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<Request>
    {
        public static Validator Instance { get; } = new();

        public Validator()
        {
            RuleFor(m => m.Name).NotEmpty().MaximumLength(42);
        }
    }

    public class Action : IRequestHandler<Request, Result<Unit>>
    {
        private IAppDbContextFactory AppDbContextFactory { get; }

        public Action(IAppDbContextFactory appDbContextFactory)
        {
            AppDbContextFactory = appDbContextFactory ?? throw new ArgumentNullException(nameof(appDbContextFactory));
        }

        public async Task<Result<Unit>> Handle(Request request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var validationResult = Validator.Instance.Validate(request);
            if (!validationResult.IsValid)
                return Error<Unit>(validationResult.Errors[0].ErrorMessage);

            await using var dbContext = await AppDbContextFactory.CreateDbContextAsync(cancellationToken);

            var existingPartType =
                await dbContext.PartTypes
                .FirstOrDefaultAsync(dbPartType => dbPartType.Id == request.Id, cancellationToken);

            if (existingPartType is null)
                return Error<Unit>("Part type not found.");

            existingPartType.Name = request.Name;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Ok(Unit.Value);
        }
    }
}
