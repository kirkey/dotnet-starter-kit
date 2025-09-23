using System.ComponentModel;

namespace Accounting.Application.Projects.Update.v1;

/// <summary>
/// Command to update an existing project with validation and domain event publishing.
/// </summary>
/// <param name="Id">The unique identifier of the project to update</param>
/// <param name="Name">Updated project name</param>
/// <param name="StartDate">Updated project start date</param>
/// <param name="EndDate">Updated project end date (for completion/cancellation)</param>
/// <param name="BudgetedAmount">Updated approved budget amount</param>
/// <param name="Status">Updated project status (Active, Completed, On Hold, Cancelled)</param>
/// <param name="ClientName">Updated client or customer name</param>
/// <param name="ProjectManager">Updated project manager assignment</param>
/// <param name="Department">Updated owning department</param>
/// <param name="Description">Updated project description</param>
/// <param name="Notes">Updated project notes</param>
public sealed record UpdateProjectCommand(
    DefaultIdType Id,
    [property: DefaultValue("Updated Infrastructure Project")] string? Name = null,
    [property: DefaultValue("2025-09-23")] DateTime? StartDate = null,
    [property: DefaultValue(null)] DateTime? EndDate = null,
    [property: DefaultValue(150000.00)] decimal? BudgetedAmount = null,
    [property: DefaultValue("Active")] string? Status = null,
    [property: DefaultValue("Updated Client Name")] string? ClientName = null,
    [property: DefaultValue("Jane Doe")] string? ProjectManager = null,
    [property: DefaultValue("Engineering")] string? Department = null,
    [property: DefaultValue("Updated project description")] string? Description = null,
    [property: DefaultValue("Updated project notes")] string? Notes = null) : IRequest<UpdateProjectResponse>;
