using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a member's membership in a group (many-to-many relationship).
/// </summary>
/// <remarks>
/// <para><strong>Use Cases:</strong></para>
/// <list type="bullet">
///   <item><description>Track which members belong to which groups</description></item>
///   <item><description>Assign and track member roles within groups (leader, secretary, treasurer)</description></item>
///   <item><description>Record membership dates for tenure tracking</description></item>
///   <item><description>Handle member transfers between groups</description></item>
///   <item><description>Manage membership status (active, suspended, withdrawn)</description></item>
/// </list>
/// <para><strong>Business Context:</strong></para>
/// <para>
/// This entity serves as the join table between <see cref="Member"/> and <see cref="MemberGroup"/>,
/// but contains additional business data beyond a simple link:
/// </para>
/// <list type="bullet">
///   <item><description><strong>Role</strong>: Members may serve as group leaders, secretaries, or treasurers</description></item>
///   <item><description><strong>Join/Leave Dates</strong>: Track membership duration and history</description></item>
///   <item><description><strong>Status</strong>: Members can be temporarily suspended or permanently withdrawn</description></item>
/// </list>
/// <para>
/// A member may belong to only one active group at a time in most MFI policies, but historical
/// memberships are retained for audit purposes.
/// </para>
/// <para><strong>Member Roles:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Member</strong>: Regular group participant</description></item>
///   <item><description><strong>Leader</strong>: Facilitates meetings, resolves conflicts</description></item>
///   <item><description><strong>Secretary</strong>: Maintains attendance and records</description></item>
///   <item><description><strong>Treasurer</strong>: Handles group savings and collections</description></item>
/// </list>
/// <para><strong>Related Entities:</strong></para>
/// <list type="bullet">
///   <item><description><see cref="Member"/> - The member in this membership</description></item>
///   <item><description><see cref="MemberGroup"/> - The group in this membership</description></item>
/// </list>
/// </remarks>
public class GroupMembership : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for role field. (2^5 = 32)</summary>
    public const int RoleMaxLength = 32;

    /// <summary>Maximum length for status field. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for notes field. (2^11 = 2048)</summary>
    public const int NotesMaxLength = 2048;

    // Membership Roles
    public const string RoleMember = "Member";
    public const string RoleLeader = "Leader";
    public const string RoleSecretary = "Secretary";
    public const string RoleTreasurer = "Treasurer";

    // Membership Statuses
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";
    public const string StatusSuspended = "Suspended";
    public const string StatusWithdrawn = "Withdrawn";

    /// <summary>Gets the member ID.</summary>
    public DefaultIdType MemberId { get; private set; }

    /// <summary>Gets the member navigation property.</summary>
    public virtual Member Member { get; private set; } = default!;

    /// <summary>Gets the group ID.</summary>
    public DefaultIdType GroupId { get; private set; }

    /// <summary>Gets the group navigation property.</summary>
    public virtual MemberGroup Group { get; private set; } = default!;

    /// <summary>Gets the date the member joined the group.</summary>
    public DateOnly JoinDate { get; private set; }

    /// <summary>Gets the date the member left the group.</summary>
    public DateOnly? LeaveDate { get; private set; }

    /// <summary>Gets the member's role in the group.</summary>
    public string Role { get; private set; } = default!;

    /// <summary>Gets the membership status.</summary>
    public string Status { get; private set; } = default!;

    private GroupMembership() { }

    private GroupMembership(
        DefaultIdType id,
        DefaultIdType memberId,
        DefaultIdType groupId,
        DateOnly joinDate,
        string role,
        string? notes)
    {
        Id = id;
        MemberId = memberId;
        GroupId = groupId;
        JoinDate = joinDate;
        Role = role.Trim();
        Status = StatusActive;
        Notes = notes?.Trim();

        QueueDomainEvent(new GroupMembershipCreated { GroupMembership = this });
    }

    /// <summary>
    /// Creates a new GroupMembership instance.
    /// </summary>
    public static GroupMembership Create(
        DefaultIdType memberId,
        DefaultIdType groupId,
        DateOnly? joinDate = null,
        string? role = null,
        string? notes = null)
    {
        return new GroupMembership(
            DefaultIdType.NewGuid(),
            memberId,
            groupId,
            joinDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            role ?? RoleMember,
            notes);
    }

    /// <summary>
    /// Updates the membership role.
    /// </summary>
    public GroupMembership UpdateRole(string role)
    {
        if (!string.Equals(Role, role.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            Role = role.Trim();
            QueueDomainEvent(new GroupMembershipRoleChanged { MembershipId = Id, NewRole = Role });
        }
        return this;
    }

    /// <summary>
    /// Withdraws the member from the group.
    /// </summary>
    public GroupMembership Withdraw(DateOnly? leaveDate = null, string? reason = null)
    {
        if (Status == StatusWithdrawn)
            throw new InvalidOperationException("Membership is already withdrawn.");

        Status = StatusWithdrawn;
        LeaveDate = leaveDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Withdrawn: {reason}" : $"{Notes}\nWithdrawn: {reason}";
        }

        QueueDomainEvent(new GroupMembershipWithdrawn { MembershipId = Id });
        return this;
    }

    /// <summary>
    /// Suspends the membership.
    /// </summary>
    public GroupMembership Suspend(string? reason = null)
    {
        if (Status != StatusActive)
            throw new InvalidOperationException($"Cannot suspend membership in {Status} status.");

        Status = StatusSuspended;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Suspended: {reason}" : $"{Notes}\nSuspended: {reason}";
        }

        return this;
    }

    /// <summary>
    /// Reactivates the membership.
    /// </summary>
    public GroupMembership Reactivate()
    {
        if (Status == StatusWithdrawn)
            throw new InvalidOperationException("Cannot reactivate a withdrawn membership.");

        if (Status != StatusActive)
        {
            Status = StatusActive;
        }
        return this;
    }
}
