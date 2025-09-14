namespace Accounting.Application.Budgets.Dtos;

public class BudgetDto : BaseDto
{
    public DefaultIdType PeriodId { get; set; }
    public int FiscalYear { get; set; }
    public string BudgetType { get; set; } = null!;
    public decimal TotalBudgetedAmount { get; set; }
    public decimal TotalActualAmount { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
}

public class BudgetLineDto(
    DefaultIdType id,
    DefaultIdType budgetId,
    DefaultIdType accountId,
    decimal budgetedAmount,
    decimal actualAmount,
    string? description)
{
    public DefaultIdType Id { get; set; } = id;
    public DefaultIdType BudgetId { get; set; } = budgetId;
    public DefaultIdType AccountId { get; set; } = accountId;
    public decimal BudgetedAmount { get; set; } = budgetedAmount;
    public decimal ActualAmount { get; set; } = actualAmount;
    public string? Description { get; set; } = description;
}
