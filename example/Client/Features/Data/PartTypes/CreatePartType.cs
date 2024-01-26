using FluentValidation;
using MediatR;
using Tyne.Aerospace.Data.Entities;

namespace Tyne.Aerospace.Client.Features.Data.PartTypes;

public static class CreatePartType
{
    public class Request : IRequest<Result<Guid>>
    {
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

    public class Action : IRequestHandler<Request, Result<Guid>>
    {
        private IAppDbContextFactory AppDbContextFactory { get; }

        public Action(IAppDbContextFactory appDbContextFactory)
        {
            AppDbContextFactory = appDbContextFactory ?? throw new ArgumentNullException(nameof(appDbContextFactory));
        }

        public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var validationResult = Validator.Instance.Validate(request);
            if (!validationResult.IsValid)
                return Error<Guid>(validationResult.Errors[0].ErrorMessage);

            await using var dbContext = await AppDbContextFactory.CreateDbContextAsync(cancellationToken);
            var partType = new PartType
            {
                Name = request.Name
            };
            dbContext.PartTypes.Add(partType);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Ok(partType.Id);
        }
    }
}
