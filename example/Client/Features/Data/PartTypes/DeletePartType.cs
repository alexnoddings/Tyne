using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Tyne.Aerospace.Client.Features.Data.PartTypes;

public static class DeletePartType
{
    public class Request : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
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

            await using var dbContext = await AppDbContextFactory.CreateDbContextAsync(cancellationToken);

            var partTypesQueryable = dbContext.PartTypes.Where(dbPartType => dbPartType.Id == request.Id);

            var partTypeExists = await partTypesQueryable.AnyAsync(cancellationToken);
            if (partTypeExists)
            {
                await partTypesQueryable.ExecuteDeleteAsync(cancellationToken);
                return Ok(Unit.Value);
            }

            return Error<Unit>("Part type not found.");
        }
    }
}
