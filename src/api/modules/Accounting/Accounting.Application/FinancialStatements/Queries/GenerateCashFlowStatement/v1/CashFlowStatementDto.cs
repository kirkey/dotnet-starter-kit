namespace Accounting.Application.FinancialStatements.Queries.GenerateCashFlowStatement.v1;

public class CashFlowStatementDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ReportTitle { get; set; } = "Cash Flow Statement";
    public string Method { get; set; } = "Direct";
    public CashFlowSectionDto OperatingActivities { get; set; } = new();
    public CashFlowSectionDto InvestingActivities { get; set; } = new();
    public CashFlowSectionDto FinancingActivities { get; set; } = new();
    public decimal NetCashFlow => OperatingActivities.Total + InvestingActivities.Total + FinancingActivities.Total;
    public decimal BeginningCashBalance { get; set; }
    public decimal EndingCashBalance => BeginningCashBalance + NetCashFlow;
    public CashFlowStatementDto? ComparativePeriod { get; set; }
}

public class CashFlowSectionDto
{
    public string SectionName { get; set; } = string.Empty;
    public List<CashFlowLineDto> Lines { get; set; } = new();
    public decimal Total => Lines.Sum(l => l.Amount);
}

public class CashFlowLineDto
{
    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public decimal? ComparativeAmount { get; set; }
    public decimal? Change => ComparativeAmount.HasValue ? Amount - ComparativeAmount.Value : null;
}
