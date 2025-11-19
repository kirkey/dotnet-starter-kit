namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Get.v1;

public sealed record PayrollDeductionResponse(
    DefaultIdType Id,
    DefaultIdType PayComponentId,
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
    DefaultIdType? OrganizationalUnitId,
    string? ReferenceNumber,
    string? Remarks);

