namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Create.v1;

/// <summary>
/// Command to create a new employee with Philippines Labor Code compliance.
/// Includes mandatory government IDs (TIN, SSS, PhilHealth, Pag-IBIG) and personal information.
/// </summary>
public sealed record CreateEmployeeCommand(
    [property: DefaultValue("EMP-001")] string EmployeeNumber,
    [property: DefaultValue("John")] string FirstName,
    [property: DefaultValue("Doe")] string LastName,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType OrganizationalUnitId,
    
    // Optional Basic Info
    [property: DefaultValue(null)] string? MiddleName = null,
    [property: DefaultValue("john.doe@example.com")] string? Email = null,
    [property: DefaultValue("+639171234567")] string? PhoneNumber = null,
    [property: DefaultValue(null)] DateTime? HireDate = null,
    
    // Philippines-Specific: Personal Information
    [property: DefaultValue(null)] DateTime? BirthDate = null,
    [property: DefaultValue("Male")] string? Gender = null,
    [property: DefaultValue("Single")] string? CivilStatus = null,
    
    // Philippines-Specific: Government IDs (Mandatory)
    [property: DefaultValue(null)] string? Tin = null,
    [property: DefaultValue(null)] string? SssNumber = null,
    [property: DefaultValue(null)] string? PhilHealthNumber = null,
    [property: DefaultValue(null)] string? PagIbigNumber = null,
    
    // Philippines-Specific: Employment Classification per Labor Code Article 280
    [property: DefaultValue("Regular")] string EmploymentClassification = "Regular",
    [property: DefaultValue(null)] DateTime? RegularizationDate = null,
    [property: DefaultValue(null)] decimal? BasicMonthlySalary = null,
    
    // Philippines-Specific: Special Status
    [property: DefaultValue(false)] bool IsPwd = false,
    [property: DefaultValue(null)] string? PwdIdNumber = null,
    [property: DefaultValue(false)] bool IsSoloParent = false,
    [property: DefaultValue(null)] string? SoloParentIdNumber = null
) : IRequest<CreateEmployeeResponse>;

