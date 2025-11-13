namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Create.v1;

/// <summary>
/// Command to create an employee dependent.
/// </summary>
public sealed record CreateEmployeeDependentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("John")] string FirstName,
    [property: DefaultValue("Doe")] string LastName,
    [property: DefaultValue("Child")] string DependentType,
    [property: DefaultValue("2015-03-20")] DateTime DateOfBirth,
    [property: DefaultValue(null)] string? Relationship = null,
    [property: DefaultValue(null)] string? Ssn = null,
    [property: DefaultValue(null)] string? Email = null,
    [property: DefaultValue(null)] string? PhoneNumber = null) : IRequest<CreateEmployeeDependentResponse>;

