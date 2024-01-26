using MediatR;
using Tyne.Searching;
using Tyne.Aerospace.Data.Entities;
using System.Diagnostics.CodeAnalysis;
using Tyne.Aerospace.Client.Infrastructure.Data;

namespace Tyne.Aerospace.Client.Features.Data.PartTypes;

public static class SearchPartTypes
{
    public class Request : SearchQuery, IRequest<SearchResults<Response>>
    {
        public string? Name { get; set; }
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "Not relevant for demo.")]
        [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Not relevant for demo.")]
        public List<string>? Names { get; set; }

        public DateTime? CreatedAtMin { get; set; }
        public DateTime? CreatedAtMax { get; set; }

        public DateTime? LastUpdatedAtMin { get; set; }
        public DateTime? LastUpdatedAtMax { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public string? CreatedByName { get; set; }

        public DateTime LastUpdatedAt { get; set; }
        public string? LastUpdatedByName { get; set; }
    }

    public class Action : SearchHandler<Request, Response, PartType>
    {
        public Action(IAppDbContextFactory appDbContextFactory) : base(appDbContextFactory)
        {
        }

        protected override IQueryable<PartType> Filter(IQueryable<PartType> source, Request query) =>
            source
            .Where(dbPartType => string.IsNullOrEmpty(query.Name) || dbPartType.Name.Contains(query.Name))
            .Where(dbPartType => query.Names == null || query.Names.Contains(dbPartType.Name))
            .Where(dbPartType => query.CreatedAtMin == null || dbPartType.CreatedAtUtc >= query.CreatedAtMin)
            .Where(dbPartType => query.CreatedAtMax == null || dbPartType.CreatedAtUtc <= query.CreatedAtMax)
            .Where(dbPartType => query.LastUpdatedAtMin == null || dbPartType.LastUpdatedAtUtc >= query.LastUpdatedAtMin)
            .Where(dbPartType => query.LastUpdatedAtMax == null || dbPartType.LastUpdatedAtUtc <= query.LastUpdatedAtMax);

        protected override IQueryable<Response> OrderDefault(IQueryable<Response> source) =>
            source.OrderBy(dbPartType => dbPartType.Name);

        protected override IQueryable<Response> Project(IQueryable<PartType> source, Request query) =>
            source.Select(dbPartType => new Response
            {
                Id = dbPartType.Id,
                Name = dbPartType.Name,
                CreatedAt = dbPartType.CreatedAtUtc,
                CreatedByName = dbPartType.CreatedBy != null ? dbPartType.CreatedBy.Name : null,
                LastUpdatedAt = dbPartType.LastUpdatedAtUtc,
                LastUpdatedByName = dbPartType.LastUpdatedBy != null ? dbPartType.LastUpdatedBy.Name : null,
            });
    }
}
