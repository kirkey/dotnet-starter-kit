namespace Accounting.Application.Projects.Update;

public class UpdateProjectRequest(
    DefaultIdType id,
    string? name = null,
    DateTime? startDate = null,
    DateTime? endDate = null,
    decimal? budgetedAmount = null,
    string? status = null,
    string? clientName = null,
    string? projectManager = null,
    string? department = null,
    string? description = null,
    string? notes = null)
    : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; } = id;
    public string? Name { get; set; } = name;
    public DateTime? StartDate { get; set; } = startDate;
    public DateTime? EndDate { get; set; } = endDate;
    public decimal? BudgetedAmount { get; set; } = budgetedAmount;
    public string? Status { get; set; } = status;
    public string? ClientName { get; set; } = clientName;
    public string? ProjectManager { get; set; } = projectManager;
    public string? Department { get; set; } = department;
    public string? Description { get; set; } = description;
    public string? Notes { get; set; } = notes;
}
