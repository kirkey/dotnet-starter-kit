using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a member group (solidarity group/center) in the microfinance system.
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Organize members into solidarity groups for group lending methodology</description></item>
///   <item><description>Schedule and track group meetings for loan disbursement and collection</description></item>
///   <item><description>Assign loan officers to manage groups of members</description></item>
///   <item><description>Facilitate peer pressure and mutual support among group members</description></item>
///   <item><description>Track group performance metrics and repayment rates</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// Group lending (also known as solidarity lending) is a cornerstone of microfinance methodology,
/// pioneered by Grameen Bank. Key principles:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Joint Liability</strong>: Group members guarantee each other's loans</description></item>
///   <item><description><strong>Social Collateral</strong>: Peer pressure replaces physical collateral</description></item>
///   <item><description><strong>Regular Meetings</strong>: Weekly/monthly meetings for collections and support</description></item>
///   <item><description><strong>Self-Selection</strong>: Members choose their own group partners</description></item>
///   <item><description><strong>Progressive Lending</strong>: Successful groups access larger loans</description></item>
/// </list>
/// <para>
/// Groups typically have 5-30 members, meet regularly (often weekly), and have elected leaders
/// (chairperson, secretary, treasurer). Meeting attendance and savings contributions are tracked.
/// </para>
/// <para><strong>Group Hierarchy:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Center</strong>: Collection of groups meeting at the same location/time</description></item>
///   <item><description><strong>Group</strong>: Small team of 5-10 members with joint liability</description></item>
///   <item><description><strong>Member</strong>: Individual borrower/saver</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="GroupMembership"/> - Links members to groups</description></item>
///   <item><description><see cref="Member"/> - Individual members in the group</description></item>
/// </list>
/// </remarks>
public class MemberGroup : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>Maximum length for group code field. (2^6 = 64)</summary>
    public const int CodeMaxLength = 64;

    /// <summary>Maximum length for group name field. (2^8 = 256)</summary>
    public const int NameMaxLength = 256;

    /// <summary>Maximum length for description field. (2^11 = 2048)</summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>Maximum length for meeting location field. (2^9 = 512)</summary>
    public const int MeetingLocationMaxLength = 512;

    /// <summary>Maximum length for meeting frequency field. (2^5 = 32)</summary>
    public const int MeetingFrequencyMaxLength = 32;

    /// <summary>Maximum length for meeting day field. (2^5 = 32)</summary>
    public const int MeetingDayMaxLength = 32;

    /// <summary>Maximum length for status field. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for notes field. (2^12 = 4096)</summary>
    public const int NotesMaxLength = 4096;

    /// <summary>Minimum length for group name field.</summary>
    public const int NameMinLength = 2;

    // Group Statuses
    public const string StatusPending = "Pending";
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusDissolved = "Dissolved";

    // Meeting Frequencies
    public const string FrequencyWeekly = "Weekly";
    public const string FrequencyBiweekly = "Biweekly";
    public const string FrequencyMonthly = "Monthly";

    /// <summary>Gets the unique group code.</summary>
    public string Code { get; private set; } = default!;


    /// <summary>Gets the date the group was formed.</summary>
    public DateOnly FormationDate { get; private set; }

    /// <summary>Gets the group leader member ID.</summary>
    public DefaultIdType? LeaderMemberId { get; private set; }

    /// <summary>Gets the group leader navigation property.</summary>
    public virtual Member? Leader { get; private set; }

    /// <summary>Gets the loan officer ID assigned to this group.</summary>
    public DefaultIdType? LoanOfficerId { get; private set; }

    /// <summary>Gets the meeting location.</summary>
    public string? MeetingLocation { get; private set; }

    /// <summary>Gets the meeting frequency (Weekly, Biweekly, Monthly).</summary>
    public string? MeetingFrequency { get; private set; }

    /// <summary>Gets the meeting day.</summary>
    public string? MeetingDay { get; private set; }

    /// <summary>Gets the meeting time.</summary>
    public TimeOnly? MeetingTime { get; private set; }

    /// <summary>Gets the current status of the group.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets the collection of group memberships.</summary>
    public virtual ICollection<GroupMembership> Memberships { get; private set; } = new List<GroupMembership>();

    private MemberGroup() { }

    private MemberGroup(
        DefaultIdType id,
        string code,
        string name,
        string? description,
        DateOnly formationDate,
        DefaultIdType? leaderMemberId,
        DefaultIdType? loanOfficerId,
        string? meetingLocation,
        string? meetingFrequency,
        string? meetingDay,
        TimeOnly? meetingTime,
        string? notes)
    {
        Id = id;
        Code = code.Trim();
        ValidateAndSetName(name);
        Description = description?.Trim();
        FormationDate = formationDate;
        LeaderMemberId = leaderMemberId;
        LoanOfficerId = loanOfficerId;
        MeetingLocation = meetingLocation?.Trim();
        MeetingFrequency = meetingFrequency?.Trim();
        MeetingDay = meetingDay?.Trim();
        MeetingTime = meetingTime;
        Status = StatusPending;
        Notes = notes?.Trim();

        QueueDomainEvent(new MemberGroupCreated { MemberGroup = this });
    }

    /// <summary>
    /// Creates a new MemberGroup instance using the factory method pattern.
    /// </summary>
    public static MemberGroup Create(
        string code,
        string name,
        string? description = null,
        DateOnly? formationDate = null,
        DefaultIdType? leaderMemberId = null,
        DefaultIdType? loanOfficerId = null,
        string? meetingLocation = null,
        string? meetingFrequency = null,
        string? meetingDay = null,
        TimeOnly? meetingTime = null,
        string? notes = null)
    {
        return new MemberGroup(
            DefaultIdType.NewGuid(),
            code,
            name,
            description,
            formationDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            leaderMemberId,
            loanOfficerId,
            meetingLocation,
            meetingFrequency,
            meetingDay,
            meetingTime,
            notes);
    }

    /// <summary>
    /// Updates the member group information.
    /// </summary>
    public MemberGroup Update(
        string? name,
        string? description,
        DefaultIdType? leaderMemberId,
        DefaultIdType? loanOfficerId,
        string? meetingLocation,
        string? meetingFrequency,
        string? meetingDay,
        TimeOnly? meetingTime,
        string? notes)
    {
        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            ValidateAndSetName(name);
            hasChanges = true;
        }

        if (description != Description) { Description = description?.Trim(); hasChanges = true; }
        if (leaderMemberId != LeaderMemberId) { LeaderMemberId = leaderMemberId; hasChanges = true; }
        if (loanOfficerId != LoanOfficerId) { LoanOfficerId = loanOfficerId; hasChanges = true; }
        if (meetingLocation != MeetingLocation) { MeetingLocation = meetingLocation?.Trim(); hasChanges = true; }
        if (meetingFrequency != MeetingFrequency) { MeetingFrequency = meetingFrequency?.Trim(); hasChanges = true; }
        if (meetingDay != MeetingDay) { MeetingDay = meetingDay?.Trim(); hasChanges = true; }
        if (meetingTime != MeetingTime) { MeetingTime = meetingTime; hasChanges = true; }
        if (notes != Notes) { Notes = notes?.Trim(); hasChanges = true; }

        if (hasChanges)
        {
            QueueDomainEvent(new MemberGroupUpdated { MemberGroup = this });
        }

        return this;
    }

    /// <summary>Activates the member group.</summary>
    public MemberGroup Activate()
    {
        if (Status != StatusActive)
        {
            Status = StatusActive;
            QueueDomainEvent(new MemberGroupActivated { GroupId = Id });
        }
        return this;
    }

    /// <summary>Deactivates the member group.</summary>
    public MemberGroup Deactivate()
    {
        if (Status == StatusActive)
        {
            Status = StatusInactive;
            QueueDomainEvent(new MemberGroupDeactivated { GroupId = Id });
        }
        return this;
    }

    /// <summary>Dissolves the member group.</summary>
    public MemberGroup Dissolve(string? reason = null)
    {
        if (Status == StatusDissolved)
            throw new InvalidOperationException("Group is already dissolved.");

        Status = StatusDissolved;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Dissolved: {reason}" : $"{Notes}\nDissolved: {reason}";
        }

        QueueDomainEvent(new MemberGroupDissolved { GroupId = Id, Reason = reason });
        return this;
    }

    private void ValidateAndSetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Group name cannot be empty.", nameof(name));

        string trimmed = name.Trim();
        if (trimmed.Length < NameMinLength)
            throw new ArgumentException($"Group name must be at least {NameMinLength} characters.", nameof(name));
        if (trimmed.Length > NameMaxLength)
            throw new ArgumentException($"Group name cannot exceed {NameMaxLength} characters.", nameof(name));

        Name = trimmed;
    }
}
