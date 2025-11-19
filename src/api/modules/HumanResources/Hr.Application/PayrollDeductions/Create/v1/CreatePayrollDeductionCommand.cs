namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Create.v1;

/// <summary>
/// Command to create a new payroll deduction configuration.
/// </summary>
public sealed record CreatePayrollDeductionCommand(
    [property: DefaultValue("3fa85f64-5717-4562-b3fc-2c963f66afa6")] DefaultIdType PayComponentId,
    [property: DefaultValue("FixedAmount")] string DeductionType,
    [property: DefaultValue(0.0)] decimal DeductionAmount = 0,
    [property: DefaultValue(0.0)] decimal DeductionPercentage = 0,
    [property: DefaultValue("3fa85f64-5717-4562-b3fc-2c963f66afa6")] DefaultIdType? EmployeeId = null,
    [property: DefaultValue("3fa85f64-5717-4562-b3fc-2c963f66afa6")] DefaultIdType? OrganizationalUnitId = null,
    [property: DefaultValue(true)] bool IsAuthorized = false,
    [property: DefaultValue(false)] bool IsRecoverable = false,
    [property: DefaultValue("2025-01-01")] DateTime? StartDate = null,
    [property: DefaultValue("2025-12-31")] DateTime? EndDate = null,
    [property: DefaultValue(0.0)] decimal? MaxDeductionLimit = null,
    [property: DefaultValue("LOAN-2025-001")] string? ReferenceNumber = null,
    [property: DefaultValue("SSS loan deduction")] string? Remarks = null)
    : IRequest<CreatePayrollDeductionResponse>;

