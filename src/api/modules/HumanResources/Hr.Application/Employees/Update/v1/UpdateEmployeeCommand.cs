namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Update.v1;

/// <summary>
/// Command to update employee information with Philippines Labor Code compliance.
/// All fields are optional - only provided fields will be updated.
/// </summary>
public sealed record UpdateEmployeeCommand(
    DefaultIdType Id,
    
    // Basic Information
    [property: DefaultValue(null)] string? FirstName = null,
    [property: DefaultValue(null)] string? MiddleName = null,
    [property: DefaultValue(null)] string? LastName = null,
    [property: DefaultValue(null)] string? Email = null,
    [property: DefaultValue(null)] string? PhoneNumber = null,
    [property: DefaultValue(null)] string? Status = null,
    
    // Philippines-Specific: Personal Information
    [property: DefaultValue(null)] DateTime? BirthDate = null,
    [property: DefaultValue(null)] string? Gender = null,
    [property: DefaultValue(null)] string? CivilStatus = null,
    
    // Philippines-Specific: Government IDs
    [property: DefaultValue(null)] string? Tin = null,
    [property: DefaultValue(null)] string? SssNumber = null,
    [property: DefaultValue(null)] string? PhilHealthNumber = null,
    [property: DefaultValue(null)] string? PagIbigNumber = null,
    
    // Philippines-Specific: Employment Classification
    [property: DefaultValue(null)] string? EmploymentClassification = null,
    [property: DefaultValue(null)] DateTime? RegularizationDate = null,
    [property: DefaultValue(null)] decimal? BasicMonthlySalary = null,
    
    // Philippines-Specific: Special Status
    [property: DefaultValue(null)] bool? IsPwd = null,
    [property: DefaultValue(null)] string? PwdIdNumber = null,
    [property: DefaultValue(null)] bool? IsSoloParent = null,
    [property: DefaultValue(null)] string? SoloParentIdNumber = null,
    
    // Organizational Unit Transfer
    [property: DefaultValue(null)] DefaultIdType? OrganizationalUnitId = null
) : IRequest<UpdateEmployeeResponse>;

