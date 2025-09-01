namespace Accounting.Application.FinancialStatements.Queries.GenerateIncomeStatement.v1;

public class IncomeStatementDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ReportTitle { get; set; } = "Income Statement";
    public string ReportFormat { get; set; } = "Standard";
    public IncomeStatementSectionDto Revenue { get; set; } = new();
    public IncomeStatementSectionDto CostOfGoodsSold { get; set; } = new();
    public IncomeStatementSectionDto OperatingExpenses { get; set; } = new();
    public IncomeStatementSectionDto OtherIncome { get; set; } = new();
    public IncomeStatementSectionDto OtherExpenses { get; set; } = new();
    public decimal GrossProfit => Revenue.Total - CostOfGoodsSold.Total;
    public decimal OperatingIncome => GrossProfit - OperatingExpenses.Total;
    public decimal NetIncome => OperatingIncome + OtherIncome.Total - OtherExpenses.Total;
    public IncomeStatementDto? ComparativePeriod { get; set; }
}

public class IncomeStatementSectionDto
{
    public string SectionName { get; set; } = string.Empty;
    public List<IncomeStatementLineDto> Lines { get; set; } = new();
    public decimal Total => Lines.Sum(l => l.Amount);
}

public class IncomeStatementLineDto
{
    public DefaultIdType AccountId { get; set; }
    public string AccountCode { get; set; } = null!;
    public string AccountName { get; set; } = null!;
    public decimal Amount { get; set; }
    public decimal? ComparativeAmount { get; set; }
    public decimal? Variance => ComparativeAmount.HasValue ? Amount - ComparativeAmount.Value : null;
}
