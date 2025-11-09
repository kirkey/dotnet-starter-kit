namespace Accounting.Application.TrialBalance.Search.v1;

/// <summary>
/// Handler for searching trial balance reports.
/// </summary>
public sealed class TrialBalanceSearchHandler(
    IReadRepository<Domain.Entities.TrialBalance> repository,
    ILogger<TrialBalanceSearchHandler> logger)
    : IRequestHandler<TrialBalanceSearchRequest, PagedList<TrialBalanceSearchResponse>>
{
    private readonly IReadRepository<Domain.Entities.TrialBalance> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<TrialBalanceSearchHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<PagedList<TrialBalanceSearchResponse>> Handle(TrialBalanceSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Searching trial balance reports with filters");

        var spec = new TrialBalanceSearchSpec(request);
        var trialBalances = await _repository.ListAsync(spec, cancellationToken);
        var totalCount = await _repository.CountAsync(cancellationToken);

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

        _logger.LogInformation("Found {Count} trial balance reports", response.Count);

        return new PagedList<TrialBalanceSearchResponse>(
            response,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
