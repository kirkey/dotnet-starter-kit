namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.GroupMemberships;

/// <summary>
/// ViewModel for GroupMembership entity operations.
/// Represents the association between members and member groups.
/// </summary>
public class GroupMembershipViewModel
{
    /// <summary>
    /// Unique identifier for the group membership.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// The member group ID.
    /// </summary>
    public DefaultIdType MemberGroupId { get; set; }

    /// <summary>
    /// The member group name.
    /// </summary>
    public string? MemberGroupName { get; set; }

    /// <summary>
    /// The member ID.
    /// </summary>
    public DefaultIdType MemberId { get; set; }

    /// <summary>
    /// The member name.
    /// </summary>
    public string? MemberName { get; set; }

    /// <summary>
    /// The member number.
    /// </summary>
    public string? MemberNumber { get; set; }

    /// <summary>
    /// Role within the group (e.g., Member, Secretary, Treasurer, Chairman).
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Date when the member joined the group.
    /// </summary>
    public DateTime? JoinedDate { get; set; }

    /// <summary>
    /// Date when the member left the group (if applicable).
    /// </summary>
    public DateTime? LeftDate { get; set; }

    /// <summary>
    /// Whether the membership is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Additional notes about the membership.
    /// </summary>
    public string? Notes { get; set; }
}
