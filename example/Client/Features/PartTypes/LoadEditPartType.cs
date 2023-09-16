using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tyne;

namespace Tyne.Aerospace.Client.Features.PartTypes;

public static class LoadEditPartType
{
    public class Request : IRequest<Result<EditPartType.Request>>
    {
        public Guid Id { get; set; }
    }

    public class Action : IRequestHandler<Request, Result<EditPartType.Request>>
    {
        private IAppDbContextFactory AppDbContextFactory { get; }

        public Action(IAppDbContextFactory appDbContextFactory)
        {
            AppDbContextFactory = appDbContextFactory ?? throw new ArgumentNullException(nameof(appDbContextFactory));
        }

        public async Task<Result<EditPartType.Request>> Handle(Request request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            await using var dbContext = await AppDbContextFactory.CreateDbContextAsync();

            var partType =
                await dbContext.PartTypes
                .Where(dbPartType => dbPartType.Id == request.Id)
                .Select(dbPartType => new EditPartType.Request
                {
                    Id = dbPartType.Id,
                    Name = dbPartType.Name,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (partType is null)
                return Error<EditPartType.Request>("Part type not found.");

            await dbContext.SaveChangesAsync(cancellationToken);

            return Ok(partType);
        }
    }
}
