namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Get.v1;

/// <summary>
/// Response object for LeaveType entity with Philippines Labor Code compliance fields.
/// </summary>
public sealed record LeaveTypeResponse
{
    // Basic Information
    public DefaultIdType Id { get; init; }
    public string LeaveName { get; init; } = default!;
    public decimal AnnualAllowance { get; init; }
    public string AccrualFrequency { get; init; } = default!;
    public decimal AccrualDaysPerPeriod { get; init; }
    public bool IsPaid { get; init; }
    public decimal MaxCarryoverDays { get; init; }
    public int? CarryoverExpiryMonths { get; init; }
    public bool RequiresApproval { get; init; }
    public int? MinimumNoticeDay { get; init; }
    public bool IsActive { get; init; }
    public string? Description { get; init; }
    
    // Philippines-Specific: Leave Classification per Labor Code
    public string? LeaveCode { get; init; }
    public string ApplicableGender { get; init; } = "Both";
    public int MinimumServiceDays { get; init; }
    
    // Philippines-Specific: Medical Certification Requirements
    public bool RequiresMedicalCertification { get; init; }
    public int MedicalCertificateAfterDays { get; init; }
    
    // Philippines-Specific: Leave Properties per Labor Code
    public bool IsConvertibleToCash { get; init; }
    public bool IsCumulative { get; init; }
}
