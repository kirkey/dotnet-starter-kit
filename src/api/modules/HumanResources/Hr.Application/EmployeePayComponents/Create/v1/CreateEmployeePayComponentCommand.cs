namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Create.v1;

/// <summary>
/// Command to create a new employee pay component assignment.
/// </summary>
public sealed record CreateEmployeePayComponentCommand(
    [property: DefaultValue("3fa85f64-5717-4562-b3fc-2c963f66afa6")] DefaultIdType EmployeeId,
    [property: DefaultValue("3fa85f64-5717-4562-b3fc-2c963f66afa6")] DefaultIdType PayComponentId,
    [property: DefaultValue("Addition")] string AssignmentType,
    [property: DefaultValue(2000.0)] decimal? CustomRate = null,
    [property: DefaultValue(5000.0)] decimal? FixedAmount = null,
    [property: DefaultValue("BasicPay * 0.10")] string? CustomFormula = null,
    [property: DefaultValue("2025-01-01")] DateTime? EffectiveStartDate = null,
    [property: DefaultValue("2025-12-31")] DateTime? EffectiveEndDate = null,
    [property: DefaultValue(false)] bool IsOneTime = false,
    [property: DefaultValue("2025-06-15")] DateTime? OneTimeDate = null,
    [property: DefaultValue(12)] int? InstallmentCount = null,
    [property: DefaultValue(60000.0)] decimal? TotalAmount = null,
    [property: DefaultValue("LOAN-2025-001")] string? ReferenceNumber = null,
    [property: DefaultValue("Transportation allowance")] string? Remarks = null)
    : IRequest<CreateEmployeePayComponentResponse>;

