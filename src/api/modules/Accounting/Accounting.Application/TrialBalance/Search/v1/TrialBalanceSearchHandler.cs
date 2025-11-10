namespace Accounting.Application.TrialBalance.Search.v1;

/// <summary>
/// Handler for searching trial balance reports.
/// </summary>
public sealed class TrialBalanceSearchHandler(
    [FromKeyedServices("accounting:trial-balance")] IReadRepository<Domain.Entities.TrialBalance> repository,
    ILogger<TrialBalanceSearchHandler> logger)
    : IRequestHandler<TrialBalanceSearchRequest, PagedList<TrialBalanceSearchResponse>>
{
    public async Task<PagedList<TrialBalanceSearchResponse>> Handle(TrialBalanceSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Searching trial balance reports with filters");

        var spec = new TrialBalanceSearchSpec(request);
        var trialBalances = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var response = trialBalances.Select(tb => new TrialBalanceSearchResponse
        {
            Id = tb.Id,
            TrialBalanceNumber = tb.TrialBalanceNumber,
            PeriodId = tb.PeriodId,
            GeneratedDate = tb.GeneratedDate,
            PeriodStartDate = tb.PeriodStartDate,
            PeriodEndDate = tb.PeriodEndDate,
            TotalDebits = tb.TotalDebits,
            TotalCredits = tb.TotalCredits,
            IsBalanced = tb.IsBalanced,
            OutOfBalanceAmount = tb.OutOfBalanceAmount,
            Status = tb.Status,
            AccountCount = tb.AccountCount,
            FinalizedDate = tb.FinalizedDate,
            Description = tb.Description,
            CreatedOn = tb.CreatedOn.DateTime
        }).ToList();

        logger.LogInformation("Found {Count} trial balance reports", response.Count);

        return new PagedList<TrialBalanceSearchResponse>(
            response,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
