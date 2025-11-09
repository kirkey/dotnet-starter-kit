using Accounting.Application.FiscalPeriodCloses.Queries;
using Accounting.Application.FiscalPeriodCloses.Responses;

namespace Accounting.Application.FiscalPeriodCloses.Search;

/// <summary>
/// Handler for searching fiscal period closes with filters and pagination.
/// </summary>
public sealed class SearchFiscalPeriodClosesHandler(
    ILogger<SearchFiscalPeriodClosesHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<FiscalPeriodClose> repository)
    : IRequestHandler<SearchFiscalPeriodClosesRequest, PagedList<FiscalPeriodCloseResponse>>
{
    public async Task<PagedList<FiscalPeriodCloseResponse>> Handle(SearchFiscalPeriodClosesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new FiscalPeriodCloseSearchSpec(request);
        
        var closes = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} fiscal period closes out of {Total}", closes.Count, request.PageNumber, request.PageSize, totalCount);

        var items = closes.Select(close => new FiscalPeriodCloseResponse
        {
            Id = close.Id,
            CloseNumber = close.CloseNumber,
            PeriodStartDate = close.PeriodStartDate,
            PeriodEndDate = close.PeriodEndDate,
            CloseDate = close.CompletedDate,
            Status = close.Status,
            CloseType = close.CloseType,
            Description = close.Description,
            Notes = close.Notes
        }).ToList();

        return new PagedList<FiscalPeriodCloseResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
