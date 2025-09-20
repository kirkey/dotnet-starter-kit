using Accounting.Application.Consumptions.Queries;
using Accounting.Application.Consumptions.Responses;

namespace Accounting.Application.Consumptions.Handlers;

/// <summary>
/// Handler for searching consumptions with optional filters and pagination.
/// </summary>
public class SearchConsumptionHandler(IReadRepository<Consumption> repository)
    : IRequestHandler<SearchConsumptionQuery, PagedList<ConsumptionResponse>>
{
    /// <summary>
    /// Handles the query to search consumptions applying filters and pagination.
    /// </summary>
    public async Task<PagedList<ConsumptionResponse>> Handle(SearchConsumptionQuery request, CancellationToken cancellationToken)
    {
        // Load all consumptions from repository and work with an IQueryable for LINQ operations.
        var query = (await repository.ListAsync(cancellationToken)).AsQueryable();

        if (request.MeterId.HasValue)
            query = query.Where(x => x.MeterId == request.MeterId.Value);

        var billingPeriod = request.BillingPeriod?.Trim();
        if (!string.IsNullOrWhiteSpace(billingPeriod))
            query = query.Where(x => x.BillingPeriod.Contains(billingPeriod));

        if (request.FromDate.HasValue)
            query = query.Where(x => x.ReadingDate >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.ReadingDate <= request.ToDate.Value);

        // capture total count before pagination
        var totalCount = query.Count();

        // Use PageNumber/PageSize from PaginationFilter. Provide safe defaults.
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? int.MaxValue : request.PageSize;

        var skip = (long)(pageNumber - 1) * pageSize;
        if (skip > 0)
            query = query.Skip((int)skip);

        if (pageSize < int.MaxValue)
            query = query.Take(pageSize);

        var items = query.Select(entity => new ConsumptionResponse
        {
            Id = entity.Id,
            MeterId = entity.MeterId,
            ReadingDate = entity.ReadingDate,
            CurrentReading = entity.CurrentReading,
            PreviousReading = entity.PreviousReading,
            KWhUsed = entity.KWhUsed,
            BillingPeriod = entity.BillingPeriod,
            ReadingType = entity.ReadingType,
            Multiplier = entity.Multiplier,
            IsValidReading = entity.IsValidReading,
            ReadingSource = entity.ReadingSource,
            Description = entity.Description,
            Notes = entity.Notes
        }).ToList();

        return new PagedList<ConsumptionResponse>(items, pageNumber, pageSize, totalCount);
    }
}
