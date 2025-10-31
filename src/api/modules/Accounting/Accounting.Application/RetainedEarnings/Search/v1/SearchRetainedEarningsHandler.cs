using Accounting.Application.RetainedEarnings.Queries;
using Accounting.Application.RetainedEarnings.Responses;

namespace Accounting.Application.RetainedEarnings.Search.v1;

/// <summary>
/// Handler for searching retained earnings with filters.
/// </summary>
public sealed class SearchRetainedEarningsHandler(
    ILogger<SearchRetainedEarningsHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<Domain.Entities.RetainedEarnings> repository)
    : IRequestHandler<SearchRetainedEarningsRequest, List<RetainedEarningsResponse>>
{
    public async Task<List<RetainedEarningsResponse>> Handle(SearchRetainedEarningsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new RetainedEarningsSearchSpec(
            request.FiscalYear,
            request.Status,
            request.IsClosed);

        var retainedEarningsList = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} retained earnings records", retainedEarningsList.Count);

        return retainedEarningsList.Select(re => new RetainedEarningsResponse
        {
            Id = re.Id,
            FiscalYear = re.FiscalYear,
            BeginningBalance = re.OpeningBalance,
            NetIncome = re.NetIncome,
            Dividends = re.Distributions,
            EndingBalance = re.ClosingBalance,
            Status = re.Status,
            IsClosed = re.IsClosed,
            Description = re.Description
        }).ToList();
    }
}
