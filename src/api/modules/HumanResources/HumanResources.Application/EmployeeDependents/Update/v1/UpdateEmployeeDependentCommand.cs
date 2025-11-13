namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Update.v1;

/// <summary>
/// Command to update employee dependent.
/// </summary>
public sealed record UpdateEmployeeDependentCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? FirstName = null,
    [property: DefaultValue(null)] string? LastName = null,
    [property: DefaultValue(null)] string? Relationship = null,
    [property: DefaultValue(null)] string? Email = null,
    [property: DefaultValue(null)] string? PhoneNumber = null,
    [property: DefaultValue(false)] bool? IsBeneficiary = null,
    [property: DefaultValue(true)] bool? IsClaimableDependent = null,
    [property: DefaultValue(null)] DateTime? EligibilityEndDate = null) : IRequest<UpdateEmployeeDependentResponse>;

