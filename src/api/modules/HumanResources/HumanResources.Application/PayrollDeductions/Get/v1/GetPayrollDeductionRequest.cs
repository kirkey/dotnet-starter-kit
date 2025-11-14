namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Get.v1;

/// <summary>
/// Request to get payroll deduction details.
/// </summary>
public sealed record GetPayrollDeductionRequest(DefaultIdType Id) : IRequest<PayrollDeductionResponse>;

/// <summary>
/// Response with payroll deduction details.
/// </summary>
public sealed record PayrollDeductionResponse(
    DefaultIdType Id,
    DefaultIdType PayComponentId,
    string ComponentName,
    string DeductionType,
    decimal DeductionAmount,
    decimal DeductionPercentage,
    bool IsActive,
    bool IsAuthorized,
    bool IsRecoverable,
    DateTime StartDate,
    DateTime? EndDate,
    decimal? MaxDeductionLimit,
    DefaultIdType? EmployeeId,
    string? EmployeeName,
    DefaultIdType? OrganizationalUnitId,
    string? AreaName,
    string? ReferenceNumber,
    string? Remarks);

