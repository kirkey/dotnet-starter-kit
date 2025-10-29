namespace FSH.Starter.WebApi.Messaging.Domain;

/// <summary>
/// Represents a member of a conversation with their role and status.
/// </summary>
public sealed class ConversationMember : AuditableEntity
{
    private ConversationMember() { }

    private ConversationMember(DefaultIdType conversationId, DefaultIdType userId, string role)
    {
        ConversationId = conversationId;
        UserId = userId;
        Role = role;
        IsActive = true;
        JoinedAt = DateTime.UtcNow;
        LastReadAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets or sets the conversation ID.
    /// </summary>
    public DefaultIdType ConversationId { get; private set; }

    /// <summary>
    /// Gets or sets the navigation property to the conversation.
    /// </summary>
    public Conversation Conversation { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public DefaultIdType UserId { get; private set; }

    /// <summary>
    /// Gets or sets the member's role: "admin" or "member".
    /// </summary>
    public string Role { get; private set; } = default!;

    /// <summary>
    /// Gets or sets whether the member is active in the conversation.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets or sets when the member joined the conversation.
    /// </summary>
    public DateTime JoinedAt { get; private set; }

    /// <summary>
    /// Gets or sets when the member left the conversation (if applicable).
    /// </summary>
    public DateTime? LeftAt { get; private set; }

    /// <summary>
    /// Gets or sets when the member last read messages.
    /// </summary>
    public DateTime? LastReadAt { get; private set; }

    /// <summary>
    /// Gets or sets whether notifications are muted for this member.
    /// </summary>
    public bool IsMuted { get; private set; }

    /// <summary>
    /// Creates a new conversation member.
    /// </summary>
    public static ConversationMember Create(DefaultIdType conversationId, DefaultIdType userId, string role)
    {
        return new ConversationMember(conversationId, userId, role);
    }

    /// <summary>
    /// Updates the member's role.
    /// </summary>
    public void UpdateRole(string role)
    {
        if (!string.IsNullOrWhiteSpace(role) && Role != role)
        {
            Role = role;
        }
    }

    /// <summary>
    /// Updates the last read timestamp.
    /// </summary>
    public void UpdateLastRead(DateTime timestamp)
    {
        LastReadAt = timestamp;
    }

    /// <summary>
    /// Toggles the mute status.
    /// </summary>
    public void ToggleMute()
    {
        IsMuted = !IsMuted;
    }

    /// <summary>
    /// Deactivates the member (removes from conversation).
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        LeftAt = DateTime.UtcNow;
    }
}

