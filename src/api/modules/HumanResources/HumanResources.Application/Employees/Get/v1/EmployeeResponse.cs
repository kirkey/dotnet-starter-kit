namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;

/// <summary>
/// Response for employee details.
/// </summary>
public sealed record EmployeeResponse
{
    public DefaultIdType Id { get; init; }
    public string EmployeeNumber { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string? MiddleName { get; init; }
    public string LastName { get; init; } = default!;
    public string FullName { get; init; } = default!;
    public DefaultIdType OrganizationalUnitId { get; init; }
    public string? OrganizationalUnitName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public DateTime? HireDate { get; init; }
    public string Status { get; init; } = default!;
    public DateTime? TerminationDate { get; init; }
    public string? TerminationReason { get; init; }
    public bool IsActive { get; init; }
}

