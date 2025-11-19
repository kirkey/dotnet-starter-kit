namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Search.v1;

/// <summary>
/// Request to search payroll deductions.
/// </summary>
public sealed record SearchPayrollDeductionsRequest(
    [property: DefaultValue(null)] DefaultIdType? EmployeeId = null,
    [property: DefaultValue(null)] DefaultIdType? OrganizationalUnitId = null,
    [property: DefaultValue(null)] DefaultIdType? PayComponentId = null,
    [property: DefaultValue(null)] string? DeductionType = null,
    [property: DefaultValue(true)] bool? IsActive = null,
    [property: DefaultValue(null)] bool? IsAuthorized = null,
    [property: DefaultValue(1)] int PageNumber = 1,
    [property: DefaultValue(10)] int PageSize = 10
) : IRequest<PagedList<PayrollDeductionDto>>;

/// <summary>
/// DTO for search results.
/// </summary>
public sealed record PayrollDeductionDto(
    DefaultIdType Id,
    string ComponentName,
    string DeductionType,
    decimal Amount,
    bool IsActive,
    bool IsAuthorized,
    string Scope); // "Employee", "Area", or "Company"

