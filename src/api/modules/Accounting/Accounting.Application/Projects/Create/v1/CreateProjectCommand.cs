using System.ComponentModel;

namespace Accounting.Application.Projects.Create.v1;

/// <summary>
/// Command to create a new project with budget tracking and job costing capabilities.
/// </summary>
/// <param name="Name">Project name (required)</param>
/// <param name="StartDate">Project start date</param>
/// <param name="BudgetedAmount">Approved budget amount (must be non-negative)</param>
/// <param name="ClientName">Optional client or customer name</param>
/// <param name="ProjectManager">Optional project manager assignment</param>
/// <param name="Department">Optional owning department</param>
/// <param name="Description">Optional project description</param>
/// <param name="Notes">Optional project notes</param>
public sealed record CreateProjectCommand(
    [property: DefaultValue("Infrastructure Upgrade Project")] string Name,
    [property: DefaultValue("2025-09-23")] DateTime StartDate,
    [property: DefaultValue(100000.00)] decimal BudgetedAmount,
    [property: DefaultValue("Engineering Department")] string? ClientName = null,
    [property: DefaultValue("John Smith")] string? ProjectManager = null,
    [property: DefaultValue("Operations")] string? Department = null,
    [property: DefaultValue("Major infrastructure upgrade for facility improvements")] string? Description = null,
    [property: DefaultValue("High priority project with strict timeline")] string? Notes = null) : IRequest<CreateProjectResponse>;
