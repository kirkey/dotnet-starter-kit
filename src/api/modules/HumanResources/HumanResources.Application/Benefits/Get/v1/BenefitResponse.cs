namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Get.v1;

/// <summary>
/// Response object for Benefit details.
/// </summary>
public sealed record BenefitResponse
{
    /// <summary>
    /// Gets the unique identifier of the benefit.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the name of the benefit.
    /// </summary>
    public string BenefitName { get; init; } = default!;

    /// <summary>
    /// Gets the type of benefit (Health, Retirement, Life Insurance, etc).
    /// </summary>
    public string BenefitType { get; init; } = default!;

    /// <summary>
    /// Gets the employee contribution amount.
    /// </summary>
    public decimal EmployeeContribution { get; init; }

    /// <summary>
    /// Gets the employer contribution amount.
    /// </summary>
    public decimal EmployerContribution { get; init; }

    /// <summary>
    /// Gets a value indicating whether benefit is required.
    /// </summary>
    public bool IsRequired { get; init; }

    /// <summary>
    /// Gets a value indicating whether benefit is active.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets the description of the benefit.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the annual coverage limit.
    /// </summary>
    public decimal? AnnualLimit { get; init; }

    /// <summary>
    /// Gets a value indicating whether carryover is allowed.
    /// </summary>
    public bool IsCarryoverAllowed { get; init; }

    /// <summary>
    /// Gets the minimum eligible employees.
    /// </summary>
    public int? MinimumEligibleEmployees { get; init; }

    /// <summary>
    /// Gets the pay component ID for payroll deduction.
    /// </summary>
    public DefaultIdType? PayComponentId { get; init; }
}

