namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;

/// <summary>
/// Employee response with Philippines Labor Code compliant fields.
/// </summary>
public sealed record EmployeeResponse
{
    // Basic Information
    public DefaultIdType Id { get; init; }
    public string EmployeeNumber { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string? MiddleName { get; init; }
    public string LastName { get; init; } = default!;
    public string FullName { get; init; } = default!;
    public DefaultIdType OrganizationalUnitId { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public DateTime? HireDate { get; init; }
    public string Status { get; init; } = default!;
    
    // Philippines-Specific: Personal Information
    public DateTime? BirthDate { get; init; }
    public string? Gender { get; init; }
    public string? CivilStatus { get; init; }
    public int? Age => BirthDate.HasValue ? (int)((DateTime.Today - BirthDate.Value).TotalDays / 365.25) : null;
    
    // Philippines-Specific: Government IDs
    public string? Tin { get; init; }
    public string? SssNumber { get; init; }
    public string? PhilHealthNumber { get; init; }
    public string? PagIbigNumber { get; init; }
    
    // Philippines-Specific: Employment Classification
    public string EmploymentClassification { get; init; } = "Regular";
    public DateTime? RegularizationDate { get; init; }
    public decimal? BasicMonthlySalary { get; init; }
    
    // Philippines-Specific: Termination Details
    public DateTime? TerminationDate { get; init; }
    public string? TerminationReason { get; init; }
    public string? TerminationMode { get; init; }
    public string? SeparationPayBasis { get; init; }
    public decimal? SeparationPayAmount { get; init; }
    
    // Philippines-Specific: Special Status
    public bool IsPwd { get; init; }
    public string? PwdIdNumber { get; init; }
    public bool IsSoloParent { get; init; }
    public string? SoloParentIdNumber { get; init; }
    
    // Profile & Display
    public string? ImageUrl { get; init; }
    
    // Status
    public bool IsActive { get; init; }
}

