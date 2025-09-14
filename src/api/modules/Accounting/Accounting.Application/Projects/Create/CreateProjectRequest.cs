namespace Accounting.Application.Projects.Create;

public class CreateProjectRequest(
    string name,
    DateTime startDate,
    decimal budgetedAmount,
    string? clientName = null,
    string? projectManager = null,
    string? department = null,
    string? description = null,
    string? notes = null) : IRequest<DefaultIdType>
{
    public string Name { get; set; } = name;
    public DateTime StartDate { get; set; } = startDate;
    public decimal BudgetedAmount { get; set; } = budgetedAmount;
    public string? ClientName { get; set; } = clientName;
    public string? ProjectManager { get; set; } = projectManager;
    public string? Department { get; set; } = department;
    public string? Description { get; set; } = description;
    public string? Notes { get; set; } = notes;
}
