namespace Accounting.Application.FinancialStatements.Queries.GenerateBalanceSheet.v1;

public class BalanceSheetDto
{
    public DateTime AsOfDate { get; set; }
    public string ReportTitle { get; set; } = "Balance Sheet";
    public string ReportFormat { get; set; } = "Standard";
    public BalanceSheetSectionDto Assets { get; set; } = new();
    public BalanceSheetSectionDto Liabilities { get; set; } = new();
    public BalanceSheetSectionDto Equity { get; set; } = new();
    public decimal TotalAssets => Assets.Total;
    public decimal TotalLiabilitiesAndEquity => Liabilities.Total + Equity.Total;
    public bool IsBalanced => Math.Abs(TotalAssets - TotalLiabilitiesAndEquity) < 0.01m;
    public BalanceSheetDto? ComparativePeriod { get; set; }
}

public class BalanceSheetSectionDto
{
    public string SectionName { get; set; } = string.Empty;
    public List<BalanceSheetSubSectionDto> SubSections { get; set; } = new();
    public decimal Total => SubSections.Sum(s => s.Total);
}

public class BalanceSheetSubSectionDto
{
    public string SubSectionName { get; set; } = string.Empty;
    public List<BalanceSheetLineDto> Lines { get; set; } = new();
    public decimal Total => Lines.Sum(l => l.Amount);
}

public class BalanceSheetLineDto
{
    public DefaultIdType AccountId { get; set; }
    public string AccountCode { get; set; } = null!;
    public string AccountName { get; set; } = null!;
    public decimal Amount { get; set; }
    public decimal? ComparativeAmount { get; set; }
    public decimal? Change => ComparativeAmount.HasValue ? Amount - ComparativeAmount.Value : null;
}
