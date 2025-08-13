namespace Accounting.Application.Projects.Dtos;

public class ProjectDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal BudgetedAmount { get; set; }
    public string Status { get; set; } = null!;
    public string? ClientName { get; set; }
    public string? ProjectManager { get; set; }
    public string? Department { get; set; }
    public decimal ActualCost { get; set; }
    public decimal ActualRevenue { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public ProjectDto(
        DefaultIdType id,
        string name,
        DateTime startDate,
        DateTime? endDate,
        decimal budgetedAmount,
        string status,
        string? clientName,
        string? projectManager,
        string? department,
        decimal actualCost,
        decimal actualRevenue,
        string? description,
        string? notes)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        BudgetedAmount = budgetedAmount;
        Status = status;
        ClientName = clientName;
        ProjectManager = projectManager;
        Department = department;
        ActualCost = actualCost;
        ActualRevenue = actualRevenue;
        Description = description;
        Notes = notes;
    }
}

public class JobCostingEntryDto
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType ProjectId { get; set; }
    public DateTime EntryDate { get; set; }
    public decimal Amount { get; set; }
    public string CostType { get; set; } = null!;
    public string? Reference { get; set; }
    public string? Description { get; set; }

    public JobCostingEntryDto(
        DefaultIdType id,
        DefaultIdType projectId,
        DateTime entryDate,
        decimal amount,
        string costType,
        string? reference,
        string? description)
    {
        Id = id;
        ProjectId = projectId;
        EntryDate = entryDate;
        Amount = amount;
        CostType = costType;
        Reference = reference;
        Description = description;
    }
}
