namespace FSH.Starter.Blazor.Client.Pages.Accounting.Projects;

/// <summary>
/// View model used by the Projects EntityTable to bind form inputs and map to API commands.
/// Matches fields from ProjectResponse, CreateProjectCommand, and UpdateProjectCommand.
/// </summary>
public class ProjectViewModel
{
    /// <summary>
    /// Unique identifier of the project.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Project name (required by backend validator).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Optional notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// The server maintains status (read-only in form).
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Start date of the project (required by backend validator).
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Optional end date when completed/cancelled.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Approved budget amount; must be greater than zero per backend validator.
    /// </summary>
    public decimal BudgetedAmount { get; set; }

    /// <summary>
    /// Optional client name associated with the project.
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// Optional project manager name.
    /// </summary>
    public string? ProjectManager { get; set; }

    /// <summary>
    /// Optional department.
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// Read-only actual cost to date (displayed on edit).
    /// </summary>
    public decimal ActualCost { get; set; }

    /// <summary>
    /// Read-only actual revenue to date (displayed on edit).
    /// </summary>
    public decimal ActualRevenue { get; set; }

    /// <summary>
    /// Image URL for project logo or reference image.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Image file upload command for uploading project image.
    /// </summary>
    public FileUploadCommand? Image { get; set; }
}
