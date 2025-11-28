namespace FSH.Starter.Blazor.Client.Pages.Hr.Benefits;

/// <summary>
/// View model for Benefit CRUD operations.
/// Contains all properties needed for create and update operations.
/// </summary>
public class BenefitViewModel
{
    /// <summary>
    /// Gets or sets the benefit ID.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the benefit name.
    /// </summary>
    public string? BenefitName { get; set; }

    /// <summary>
    /// Gets or sets the benefit type.
    /// </summary>
    public string? BenefitType { get; set; } = "Health";

    /// <summary>
    /// Gets or sets the employee contribution amount.
    /// </summary>
    public decimal EmployeeContribution { get; set; }

    /// <summary>
    /// Gets or sets the employer contribution amount.
    /// </summary>
    public decimal EmployerContribution { get; set; }

    /// <summary>
    /// Gets or sets whether this is a mandatory benefit.
    /// </summary>
    public bool IsMandatory { get; set; }

    /// <summary>
    /// Gets or sets whether this benefit is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the coverage type.
    /// </summary>
    public string? CoverageType { get; set; }

    /// <summary>
    /// Gets or sets the provider name.
    /// </summary>
    public string? ProviderName { get; set; }

    /// <summary>
    /// Gets or sets the coverage amount.
    /// </summary>
    public decimal? CoverageAmount { get; set; }

    /// <summary>
    /// Gets or sets the waiting period in days.
    /// </summary>
    public int? WaitingPeriodDays { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets the total contribution (employee + employer).
    /// </summary>
    public decimal TotalContribution => EmployeeContribution + EmployerContribution;
}
