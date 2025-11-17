using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Messaging.Domain.Events;

namespace FSH.Starter.WebApi.Messaging.Domain;

/// <summary>
/// Represents a conversation that can be either a direct message between two users
/// or a group chat with multiple participants.
/// </summary>
public sealed class Conversation : AuditableEntity, IAggregateRoot
{
    private readonly List<ConversationMember> _members = new();
    private readonly List<Message> _messages = new();

    private Conversation() { }

    private Conversation(
        string? title,
        string conversationType,
        DefaultIdType? createdByUserId,
        string? description = null,
        string? imageUrl = null)
    {
        Title = title;
        ConversationType = conversationType;
        Description = description;
        ImageUrl = imageUrl;
        CreatedByUserId = createdByUserId;
        IsActive = true;

        QueueDomainEvent(new ConversationCreated(Id, Title, ConversationType));
        MessagingMetrics.ConversationCreated.Add(1);
    }

    /// <summary>
    /// Gets or sets the conversation title (optional for direct messages, required for groups).
    /// </summary>
    public string? Title { get; private set; }

    /// <summary>
    /// Gets or sets the conversation type: "direct" or "group".
    /// </summary>
    public string ConversationType { get; private set; } = default!;
    
    /// <summary>
    /// Gets or sets the user ID of the conversation creator.
    /// </summary>
    public DefaultIdType? CreatedByUserId { get; private set; }

    /// <summary>
    /// Gets or sets whether the conversation is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets or sets the timestamp of the last message in the conversation.
    /// </summary>
    public DateTime? LastMessageAt { get; private set; }

    /// <summary>
    /// Gets the collection of conversation members.
    /// </summary>
    public IReadOnlyCollection<ConversationMember> Members => _members.AsReadOnly();

    /// <summary>
    /// Gets the collection of messages in the conversation.
    /// </summary>
    public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

    /// <summary>
    /// Creates a new direct conversation between two users.
    /// </summary>
    public static Conversation CreateDirect(DefaultIdType userId1, DefaultIdType userId2, DefaultIdType createdByUserId)
    {
        var conversation = new Conversation(null, ConversationTypes.Direct, createdByUserId);
        conversation.AddMember(userId1);
        conversation.AddMember(userId2);
        return conversation;
    }

    /// <summary>
    /// Creates a new group conversation.
    /// </summary>
    public static Conversation CreateGroup(
        string title,
        DefaultIdType createdByUserId,
        string? description = null,
        string? imageUrl = null)
    {
        var conversation = new Conversation(title, ConversationTypes.Group, createdByUserId, description, imageUrl);
        conversation.AddMember(createdByUserId, MemberRoles.Admin);
        return conversation;
    }

    /// <summary>
    /// Updates the conversation details.
    /// </summary>
    public Conversation Update(string? title, string? description, string? imageUrl)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(title) && !string.Equals(Title, title, StringComparison.OrdinalIgnoreCase))
        {
            Title = title;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (!string.Equals(ImageUrl, imageUrl, StringComparison.OrdinalIgnoreCase))
        {
            ImageUrl = imageUrl;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ConversationUpdated(Id, Title, Description));
            MessagingMetrics.ConversationUpdated.Add(1);
        }

        return this;
    }

    /// <summary>
    /// Adds a member to the conversation.
    /// </summary>
    public void AddMember(DefaultIdType userId, string role = MemberRoles.Member)
    {
        if (_members.Any(m => m.UserId == userId && m.IsActive))
        {
            return; // Member already exists
        }

        var member = ConversationMember.Create(Id, userId, role);
        _members.Add(member);

        QueueDomainEvent(new MemberAdded(Id, userId, role));
        MessagingMetrics.MemberAdded.Add(1);
    }

    /// <summary>
    /// Removes a member from the conversation.
    /// </summary>
    public void RemoveMember(DefaultIdType userId)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId && m.IsActive);
        if (member == null)
        {
            return;
        }

        member.Deactivate();
        QueueDomainEvent(new MemberRemoved(Id, userId));
        MessagingMetrics.MemberRemoved.Add(1);
    }

    /// <summary>
    /// Assigns or revokes admin role for a member.
    /// </summary>
    public void UpdateMemberRole(DefaultIdType userId, string role)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId && m.IsActive);
        if (member == null)
        {
            return;
        }

        member.UpdateRole(role);
        QueueDomainEvent(new MemberRoleUpdated(Id, userId, role));
        MessagingMetrics.MemberRoleUpdated.Add(1);
    }

    /// <summary>
    /// Updates the last message timestamp.
    /// </summary>
    public void UpdateLastMessageAt(DateTime timestamp)
    {
        LastMessageAt = timestamp;
    }

    /// <summary>
    /// Deactivates the conversation.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        QueueDomainEvent(new ConversationDeleted(Id));
    }
}

/// <summary>
/// Defines the available conversation types.
/// </summary>
public static class ConversationTypes
{
    public const string Direct = "direct";
    public const string Group = "group";
}

/// <summary>
/// Defines the available member roles in a conversation.
/// </summary>
public static class MemberRoles
{
    public const string Admin = "admin";
    public const string Member = "member";
}

