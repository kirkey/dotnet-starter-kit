namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Update.v1;

/// <summary>
/// Command to update payroll deduction with Philippines Labor Code compliance.
/// </summary>
public sealed record UpdatePayrollDeductionCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] decimal? DeductionAmount = null,
    [property: DefaultValue(null)] decimal? DeductionPercentage = null,
    [property: DefaultValue(null)] DateTime? EndDate = null,
    [property: DefaultValue(null)] decimal? MaxDeductionLimit = null,
    [property: DefaultValue(null)] string? Remarks = null,
    [property: DefaultValue(null)] bool? IsActive = null
) : IRequest<UpdatePayrollDeductionResponse>;

/// <summary>
/// Response for payroll deduction update.
/// </summary>
public sealed record UpdatePayrollDeductionResponse(
    DefaultIdType Id,
    string DeductionType,
    decimal Amount,
    bool IsActive);

