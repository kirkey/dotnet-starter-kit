namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Get.v1;

/// <summary>
/// Response object for Deduction (PayComponent) details.
/// </summary>
public sealed record DeductionResponse
{
    /// <summary>
    /// Gets the unique identifier of the deduction.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the name of the deduction component.
    /// </summary>
    public string ComponentName { get; init; } = default!;

    /// <summary>
    /// Gets the component type (Earnings, Tax, Deduction).
    /// </summary>
    public string ComponentType { get; init; } = default!;

    /// <summary>
    /// Gets the GL account code for posting.
    /// </summary>
    public string GlAccountCode { get; init; } = default!;

    /// <summary>
    /// Gets a value indicating whether the deduction is active.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets a value indicating whether the deduction is auto-calculated.
    /// </summary>
    public bool IsCalculated { get; init; }

    /// <summary>
    /// Gets the description of the deduction.
    /// </summary>
    public string? Description { get; init; }
}
