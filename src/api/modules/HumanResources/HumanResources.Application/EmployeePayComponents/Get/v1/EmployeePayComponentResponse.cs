namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Get.v1;

public sealed record EmployeePayComponentResponse(
    DefaultIdType Id,
    DefaultIdType EmployeeId,
    DefaultIdType PayComponentId,
    string AssignmentType,
    decimal? CustomRate,
    decimal? FixedAmount,
    string? CustomFormula,
    DateTime EffectiveStartDate,
    DateTime? EffectiveEndDate,
    bool IsActive,
    bool IsRecurring,
    bool IsOneTime,
    DateTime? OneTimeDate,
    int? InstallmentCount,
    int? CurrentInstallment,
    decimal? TotalAmount,
    decimal? RemainingBalance,
    string? ReferenceNumber,
    DefaultIdType? ApprovedBy,
    DateTime? ApprovedDate,
    string? Remarks);

