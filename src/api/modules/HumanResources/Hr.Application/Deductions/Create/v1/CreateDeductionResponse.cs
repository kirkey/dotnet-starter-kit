namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Create.v1;

/// <summary>
/// Response for creating a deduction.
/// </summary>
/// <param name="Id">The identifier of the created deduction.</param>
/// <param name="DeductionName">The name of the deduction.</param>
/// <param name="DeductionType">The type of deduction.</param>
/// <param name="IsActive">Whether the deduction is active.</param>
public sealed record CreateDeductionResponse(
    DefaultIdType Id,
    string DeductionName,
    string DeductionType,
    bool IsActive);

