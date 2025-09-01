namespace Accounting.Application.Budgets.Queries.GenerateBudgetVarianceAnalysis.v1;

public class BudgetVarianceAnalysisDto
{
    public DefaultIdType BudgetId { get; set; }
    public string BudgetName { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string AnalysisType { get; set; } = "Detailed";
    public List<BudgetVarianceLineDto> VarianceLines { get; set; } = new();
    public decimal TotalBudgetedAmount { get; set; }
    public decimal TotalActualAmount { get; set; }
    public decimal TotalVariance => TotalActualAmount - TotalBudgetedAmount;
    public decimal TotalVariancePercentage => TotalBudgetedAmount != 0 ? (TotalVariance / TotalBudgetedAmount) * 100 : 0;
    public BudgetPerformanceSummaryDto Summary { get; set; } = new();
}

public class BudgetVarianceLineDto
{
    public DefaultIdType? AccountId { get; set; }
    public string AccountCode { get; set; } = null!;
    public string AccountName { get; set; } = null!;
    public string Category { get; set; } = null!;
    public decimal BudgetedAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public decimal Variance => ActualAmount - BudgetedAmount;
    public decimal VariancePercentage => BudgetedAmount != 0 ? (Variance / BudgetedAmount) * 100 : 0;
    public string VarianceType => Variance > 0 ? "Favorable" : Variance < 0 ? "Unfavorable" : "On Target";
}

public class BudgetPerformanceSummaryDto
{
    public int TotalAccounts { get; set; }
    public int AccountsOverBudget { get; set; }
    public int AccountsUnderBudget { get; set; }
    public int AccountsOnTarget { get; set; }
    public decimal LargestVariance { get; set; }
    public decimal SmallestVariance { get; set; }
    public decimal AverageVariancePercentage { get; set; }
}
