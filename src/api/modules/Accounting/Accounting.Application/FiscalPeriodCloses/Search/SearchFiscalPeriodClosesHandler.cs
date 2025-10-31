using Accounting.Application.FiscalPeriodCloses.Queries;
using Accounting.Application.FiscalPeriodCloses.Responses;

namespace Accounting.Application.FiscalPeriodCloses.Search;

/// <summary>
/// Handler for searching fiscal period closes with filters.
/// </summary>
public sealed class SearchFiscalPeriodClosesHandler(
    ILogger<SearchFiscalPeriodClosesHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<FiscalPeriodClose> repository)
    : IRequestHandler<SearchFiscalPeriodClosesRequest, List<FiscalPeriodCloseResponse>>
{
    public async Task<List<FiscalPeriodCloseResponse>> Handle(SearchFiscalPeriodClosesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new FiscalPeriodCloseSearchSpec(request.CloseNumber, request.CloseType, request.Status);
        var closes = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} fiscal period closes", closes.Count);

        return closes.Select(close => new FiscalPeriodCloseResponse
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
    }
}
