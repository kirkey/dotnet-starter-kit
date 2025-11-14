namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Create.v1;

/// <summary>
/// Command to create a new employee dependent.
/// </summary>
public sealed record CreateEmployeeDependentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("Jack")] string FirstName,
    [property: DefaultValue("Doe")] string LastName,
    [property: DefaultValue("Child")] string DependentType,
    [property: DefaultValue("2015-01-01")] DateTime DateOfBirth,
    [property: DefaultValue("Biological Child")] string? Relationship = null,
    [property: DefaultValue(null)] string? Email = null,
    [property: DefaultValue(null)] string? PhoneNumber = null) : IRequest<CreateEmployeeDependentResponse>;

