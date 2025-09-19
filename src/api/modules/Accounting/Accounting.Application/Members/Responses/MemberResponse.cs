namespace Accounting.Application.Members.Responses;

/// <summary>
/// Response model representing a member entity.
/// Contains member information including contact details, service information, and account status.
/// </summary>
public class MemberResponse
{
    /// <summary>
    /// Unique identifier for the member.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Unique member number for identification and reference.
    /// </summary>
    public string MemberNumber { get; set; } = default!;
    
    /// <summary>
    /// Full name of the member.
    /// </summary>
    public string MemberName { get; set; } = default!;
    
    /// <summary>
    /// Address where service is provided.
    /// </summary>
    public string ServiceAddress { get; set; } = default!;
    
    /// <summary>
    /// Mailing address for correspondence (if different from service address).
    /// </summary>
    public string? MailingAddress { get; set; }
    
    /// <summary>
    /// Contact information for the member.
    /// </summary>
    public string? ContactInfo { get; set; }
    
    /// <summary>
    /// Current status of the member account (e.g., "Active", "Suspended", "Inactive").
    /// </summary>
    public string AccountStatus { get; set; } = default!;
    
    /// <summary>
    /// Identifier for the meter associated with this member.
    /// </summary>
    public DefaultIdType? MeterId { get; set; }
    
    /// <summary>
    /// Identifier for the rate schedule applied to this member.
    /// </summary>
    public DefaultIdType? RateScheduleId { get; set; }
    
    /// <summary>
    /// Date when the member joined the cooperative.
    /// </summary>
    public DateTime MembershipDate { get; set; }
    
    /// <summary>
    /// Current outstanding balance on the member's account.
    /// </summary>
    public decimal CurrentBalance { get; set; }
    
    /// <summary>
    /// Indicates if the member account is currently active.
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Email address for electronic communications.
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Primary phone number for the member.
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Emergency contact information.
    /// </summary>
    public string? EmergencyContact { get; set; }
    
    /// <summary>
    /// Service class or category (e.g., "Residential", "Commercial", "Industrial").
    /// </summary>
    public string? ServiceClass { get; set; }
    
    /// <summary>
    /// Rate schedule name or code applied to this member.
    /// </summary>
    public string? RateSchedule { get; set; }
    
    /// <summary>
    /// Description or additional details about the member.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Additional notes or comments about the member.
    /// </summary>
    public string? Notes { get; set; }
}
