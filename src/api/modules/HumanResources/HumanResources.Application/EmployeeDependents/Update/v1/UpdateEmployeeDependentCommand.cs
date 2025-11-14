namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Update.v1;

/// <summary>
/// Command to update an employee dependent.
/// </summary>
public sealed record UpdateEmployeeDependentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue("Jack")] string? FirstName = null,
    [property: DefaultValue("Doe")] string? LastName = null,
    [property: DefaultValue("Biological Child")] string? Relationship = null,
    [property: DefaultValue(null)] string? Email = null,
    [property: DefaultValue(null)] string? PhoneNumber = null,
    [property: DefaultValue(false)] bool? IsBeneficiary = null,
    [property: DefaultValue(true)] bool? IsClaimableDependent = null) : IRequest<UpdateEmployeeDependentResponse>;

