using MediatR;

namespace Accounting.Application.Projects.Update;

public class UpdateProjectRequest : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? BudgetedAmount { get; set; }
    public string? Status { get; set; }
    public string? ClientName { get; set; }
    public string? ProjectManager { get; set; }
    public string? Department { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public UpdateProjectRequest(
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
        Description = description;
        Notes = notes;
    }
}
