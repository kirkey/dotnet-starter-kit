namespace Accounting.Application.FinancialStatements.Queries.GenerateBalanceSheet.v1;

public class GenerateBalanceSheetQuery : BaseRequest, IRequest<BalanceSheetDto>
{
    public DateTime AsOfDate { get; set; }
    public DefaultIdType? AccountingPeriodId { get; set; }
    public string ReportFormat { get; set; } = "Standard"; // Standard, Detailed, Summary
    public bool IncludeComparativePeriod { get; set; }
    public DateTime? ComparativeAsOfDate { get; set; }
}
