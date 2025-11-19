namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Update.v1;

/// <summary>
/// Command to update leave type with Philippines Labor Code compliance.
/// All fields are optional - only provided fields will be updated.
/// </summary>
public sealed record UpdateLeaveTypeCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    
    // Basic Configuration
    [property: DefaultValue(null)] string? LeaveName = null,
    [property: DefaultValue(null)] decimal? AnnualAllowance = null,
    [property: DefaultValue(null)] string? AccrualFrequency = null,
    [property: DefaultValue(null)] bool? IsPaid = null,
    [property: DefaultValue(null)] bool? RequiresApproval = null,
    [property: DefaultValue(null)] decimal? MaxCarryoverDays = null,
    [property: DefaultValue(null)] int? CarryoverExpiryMonths = null,
    [property: DefaultValue(null)] int? MinimumNoticeDay = null,
    [property: DefaultValue(null)] string? Description = null,
    [property: DefaultValue(null)] bool? IsActive = null,
    
    // Philippines-Specific Fields
    [property: DefaultValue(null)] string? LeaveCode = null,
    [property: DefaultValue(null)] string? ApplicableGender = null,
    [property: DefaultValue(null)] int? MinimumServiceDays = null,
    [property: DefaultValue(null)] bool? RequiresMedicalCertification = null,
    [property: DefaultValue(null)] int? MedicalCertificateAfterDays = null,
    [property: DefaultValue(null)] bool? IsConvertibleToCash = null,
    [property: DefaultValue(null)] bool? IsCumulative = null
) : IRequest<UpdateLeaveTypeResponse>;

