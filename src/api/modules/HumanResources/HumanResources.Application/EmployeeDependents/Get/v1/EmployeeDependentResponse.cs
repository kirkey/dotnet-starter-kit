namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;

/// <summary>
/// Response for employee dependent details.
/// </summary>
public sealed record EmployeeDependentResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType EmployeeId { get; init; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string FullName { get; init; } = default!;
    public string DependentType { get; init; } = default!;
    public DateTime DateOfBirth { get; init; }
    public int Age { get; init; }
    public string? Relationship { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public bool IsBeneficiary { get; init; }
    public bool IsClaimableDependent { get; init; }
    public DateTime? EligibilityEndDate { get; init; }
    public bool IsActive { get; init; }
}

