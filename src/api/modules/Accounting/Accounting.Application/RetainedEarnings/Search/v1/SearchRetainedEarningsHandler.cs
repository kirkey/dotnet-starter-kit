using Accounting.Application.RetainedEarnings.Queries;
using Accounting.Application.RetainedEarnings.Responses;

namespace Accounting.Application.RetainedEarnings.Search.v1;

/// <summary>
/// Handler for searching retained earnings with filters and pagination.
/// </summary>
public sealed class SearchRetainedEarningsHandler(
    ILogger<SearchRetainedEarningsHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<Domain.Entities.RetainedEarnings> repository)
    : IRequestHandler<SearchRetainedEarningsRequest, PagedList<RetainedEarningsResponse>>
{
    public async Task<PagedList<RetainedEarningsResponse>> Handle(SearchRetainedEarningsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Apply OnlyOpen filter if specified
        var isClosed = request.OnlyOpen ? false : request.IsClosed;

        var spec = new RetainedEarningsSearchSpec(
            request.FiscalYear,
            request.Status,
            isClosed);

        var retainedEarningsList = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} retained earnings records", retainedEarningsList.Count);

        var responseList = retainedEarningsList.Select(re => new RetainedEarningsResponse
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

        // Apply pagination
        var pagedList = responseList
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PagedList<RetainedEarningsResponse>(
            pagedList,
            responseList.Count,
            request.PageNumber,
            request.PageSize);
    }
}
