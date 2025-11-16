namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Update.v1;

/// <summary>
/// Response for updating a deduction.
/// </summary>
public sealed record UpdateDeductionResponse(
    DefaultIdType Id,
    string DeductionName,
    bool IsActive);
