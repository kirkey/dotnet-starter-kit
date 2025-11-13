namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;

/// <summary>
/// Response for employee contact details.
/// </summary>
public sealed record EmployeeContactResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType EmployeeId { get; init; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string FullName { get; init; } = default!;
    public string ContactType { get; init; } = default!;
    public string? Relationship { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
    public string? Address { get; init; }
    public int Priority { get; init; }
    public bool IsActive { get; init; }
}

