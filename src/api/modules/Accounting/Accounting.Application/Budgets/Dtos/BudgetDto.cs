namespace Accounting.Application.Budgets.Dtos;

public class BudgetDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = null!;
    public DefaultIdType PeriodId { get; set; }
    public int FiscalYear { get; set; }
    public string BudgetType { get; set; } = null!;
    public string Status { get; set; } = null!;
    public decimal TotalBudgetedAmount { get; set; }
    public decimal TotalActualAmount { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public BudgetDto(
        DefaultIdType id,
        string name,
        DefaultIdType periodId,
        int fiscalYear,
        string budgetType,
        string status,
        decimal totalBudgetedAmount,
        decimal totalActualAmount,
        DateTime? approvedDate,
        string? approvedBy,
        string? description,
        string? notes)
    {
        Id = id;
        Name = name;
        PeriodId = periodId;
        FiscalYear = fiscalYear;
        BudgetType = budgetType;
        Status = status;
        TotalBudgetedAmount = totalBudgetedAmount;
        TotalActualAmount = totalActualAmount;
        ApprovedDate = approvedDate;
        ApprovedBy = approvedBy;
        Description = description;
        Notes = notes;
    }
}

public class BudgetLineDto
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType BudgetId { get; set; }
    public DefaultIdType AccountId { get; set; }
    public decimal BudgetedAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public string? Description { get; set; }

    public BudgetLineDto(
        DefaultIdType id,
        DefaultIdType budgetId,
        DefaultIdType accountId,
        decimal budgetedAmount,
        decimal actualAmount,
        string? description)
    {
        Id = id;
        BudgetId = budgetId;
        AccountId = accountId;
        BudgetedAmount = budgetedAmount;
        ActualAmount = actualAmount;
        Description = description;
    }
}
