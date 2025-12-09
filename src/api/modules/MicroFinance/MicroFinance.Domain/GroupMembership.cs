using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a member's membership in a group (many-to-many relationship).
/// </summary>
/// <remarks>
/// Use cases:
/// - Track which members belong to which groups.
/// - Assign and track member roles within groups (leader, secretary, treasurer).
/// - Record membership dates for tenure tracking.
/// - Handle member transfers between groups.
/// - Manage membership status (active, suspended, withdrawn).
/// 
/// Default values and constraints:
/// - Role: Member, Leader, Secretary, or Treasurer (max 32 chars).
/// - Status: Active, Suspended, or Withdrawn (max 32 chars).
/// - JoinDate: Date member joined the group (required).
/// - LeaveDate: Date member left (null if still active).
/// - Notes: Additional membership notes (max 2048 chars).
/// 
/// Business rules:
/// - Serves as join table between Member and MemberGroup with business data.
/// - A member may belong to only one active group at a time.
/// - Historical memberships retained for audit purposes.
/// - Leader: Facilitates meetings, resolves conflicts.
/// - Secretary: Maintains attendance and records.
/// - Treasurer: Handles group savings and collections.
/// </remarks>
/// <seealso cref="Member"/>
/// <seealso cref="MemberGroup"/>
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
