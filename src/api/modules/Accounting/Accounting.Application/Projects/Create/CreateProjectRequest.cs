using MediatR;

namespace Accounting.Application.Projects.Create;

public class CreateProjectRequest : IRequest<DefaultIdType>
{
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public decimal BudgetedAmount { get; set; }
    public string? ClientName { get; set; }
    public string? ProjectManager { get; set; }
    public string? Department { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public CreateProjectRequest(
        string name,
        DateTime startDate,
        decimal budgetedAmount,
        string? clientName = null,
        string? projectManager = null,
        string? department = null,
        string? description = null,
        string? notes = null)
    {
        Name = name;
        StartDate = startDate;
        BudgetedAmount = budgetedAmount;
        ClientName = clientName;
        ProjectManager = projectManager;
        Department = department;
        Description = description;
        Notes = notes;
    }
}
