namespace Accounting.Application.TrialBalance.Queries.GenerateTrialBalance.v1;

public class GenerateTrialBalanceQuery : BaseRequest, IRequest<TrialBalanceDto>
{
    public DateTime AsOfDate { get; set; }
    public DefaultIdType? AccountingPeriodId { get; set; }
    public bool IncludeZeroBalances { get; set; }
    public string? AccountTypeFilter { get; set; }
}
