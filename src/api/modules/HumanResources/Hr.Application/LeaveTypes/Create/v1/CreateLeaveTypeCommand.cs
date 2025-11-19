namespace FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Create.v1;

/// <summary>
/// Command to create a new leave type with Philippines Labor Code compliance.
/// Includes mandatory leave classification, gender rules, and medical certification requirements.
/// </summary>
public sealed record CreateLeaveTypeCommand(
    [property: DefaultValue("Vacation Leave")] string LeaveName,
    [property: DefaultValue(5)] decimal AnnualAllowance,
    
    // Basic Leave Configuration
    [property: DefaultValue("Monthly")] string AccrualFrequency = "Monthly",
    [property: DefaultValue(true)] bool IsPaid = true,
    [property: DefaultValue(true)] bool RequiresApproval = true,
    [property: DefaultValue(0)] decimal MaxCarryoverDays = 0,
    [property: DefaultValue(null)] int? CarryoverExpiryMonths = null,
    [property: DefaultValue(null)] int? MinimumNoticeDay = null,
    [property: DefaultValue(null)] string? Description = null,
    
    // Philippines-Specific: Leave Classification per Labor Code
    [property: DefaultValue("VacationLeave")] string? LeaveCode = null,
    [property: DefaultValue("Both")] string ApplicableGender = "Both",
    [property: DefaultValue(0)] int MinimumServiceDays = 0,
    
    // Philippines-Specific: Medical Certification Requirements
    [property: DefaultValue(false)] bool RequiresMedicalCertification = false,
    [property: DefaultValue(0)] int MedicalCertificateAfterDays = 0,
    
    // Philippines-Specific: Leave Properties per Labor Code
    [property: DefaultValue(false)] bool IsConvertibleToCash = false,
    [property: DefaultValue(false)] bool IsCumulative = false
) : IRequest<CreateLeaveTypeResponse>;

