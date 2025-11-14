namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Create.v1;

/// <summary>
/// Command to create a new payroll deduction per Philippines Labor Code.
/// Authorizes deductions for loans, insurance, union dues, etc.
/// </summary>
public sealed record CreatePayrollDeductionCommand(
    DefaultIdType PayComponentId,
    [property: DefaultValue("FixedAmount")] string DeductionType = "FixedAmount",
    [property: DefaultValue(1000)] decimal DeductionAmount = 1000,
    [property: DefaultValue(0)] decimal DeductionPercentage = 0,
    [property: DefaultValue(null)] DefaultIdType? EmployeeId = null,
    [property: DefaultValue(null)] DefaultIdType? OrganizationalUnitId = null,
    [property: DefaultValue("2025-11-01")] DateTime StartDate = default,
    [property: DefaultValue(null)] DateTime? EndDate = null,
    [property: DefaultValue(null)] decimal? MaxDeductionLimit = null,
    [property: DefaultValue(null)] string? ReferenceNumber = null,
    [property: DefaultValue(null)] string? Remarks = null
) : IRequest<CreatePayrollDeductionResponse>;

/// <summary>
/// Response for payroll deduction creation.
/// </summary>
public sealed record CreatePayrollDeductionResponse(
    DefaultIdType Id,
    string DeductionType,
    decimal Amount,
    bool IsAuthorized);

