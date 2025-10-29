using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Messaging.Domain.Events;

namespace FSH.Starter.WebApi.Messaging.Domain;

/// <summary>
/// Represents a message in a conversation.
/// </summary>
public sealed class Message : AuditableEntity, IAggregateRoot
{
    private readonly List<MessageAttachment> _attachments = new();

    private Message() { }

    private Message(
        DefaultIdType conversationId,
        DefaultIdType senderId,
        string content,
        string messageType)
    {
        ConversationId = conversationId;
        SenderId = senderId;
        Content = content;
        MessageType = messageType;
        IsEdited = false;
        IsDeleted = false;
        SentAt = DateTime.UtcNow;

        QueueDomainEvent(new MessageSent(Id, ConversationId, SenderId, Content));
        MessagingMetrics.MessageSent.Add(1);
    }

    /// <summary>
    /// Gets or sets the conversation ID this message belongs to.
    /// </summary>
    public DefaultIdType ConversationId { get; private set; }

    /// <summary>
    /// Gets or sets the navigation property to the conversation.
    /// </summary>
    public Conversation Conversation { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the sender's user ID.
    /// </summary>
    public DefaultIdType SenderId { get; private set; }

    /// <summary>
    /// Gets or sets the message content.
    /// </summary>
    public string Content { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the message type: "text", "file", "image", "video", "audio", etc.
    /// </summary>
    public string MessageType { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the ID of the message being replied to (if any).
    /// </summary>
    public DefaultIdType? ReplyToMessageId { get; private set; }

    /// <summary>
    /// Gets or sets the navigation property to the replied message.
    /// </summary>
    public Message? ReplyToMessage { get; private set; }

    /// <summary>
    /// Gets or sets when the message was sent.
    /// </summary>
    public DateTime SentAt { get; private set; }

    /// <summary>
    /// Gets or sets whether the message has been edited.
    /// </summary>
    public bool IsEdited { get; private set; }

    /// <summary>
    /// Gets or sets when the message was edited (if applicable).
    /// </summary>
    public DateTime? EditedAt { get; private set; }

    /// <summary>
    /// Gets or sets whether the message has been deleted.
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// Gets or sets when the message was deleted (if applicable).
    /// </summary>
    public DateTime? DeletedAt { get; private set; }

    /// <summary>
    /// Gets the collection of attachments for this message.
    /// </summary>
    public IReadOnlyCollection<MessageAttachment> Attachments => _attachments.AsReadOnly();

    /// <summary>
    /// Creates a new text message.
    /// </summary>
    public static Message CreateText(
        DefaultIdType conversationId,
        DefaultIdType senderId,
        string content,
        DefaultIdType? replyToMessageId = null)
    {
        var message = new Message(conversationId, senderId, content, MessageTypes.Text);
        if (replyToMessageId.HasValue)
        {
            message.ReplyToMessageId = replyToMessageId.Value;
        }
        return message;
    }

    /// <summary>
    /// Creates a new message with attachments.
    /// </summary>
    public static Message CreateWithAttachments(
        DefaultIdType conversationId,
        DefaultIdType senderId,
        string content,
        string messageType)
    {
        return new Message(conversationId, senderId, content, messageType);
    }

    /// <summary>
    /// Updates the message content.
    /// </summary>
    public Message Update(string content)
    {
        if (!string.IsNullOrWhiteSpace(content) && !string.Equals(Content, content, StringComparison.Ordinal))
        {
            Content = content;
            IsEdited = true;
            EditedAt = DateTime.UtcNow;

            QueueDomainEvent(new MessageUpdated(Id, ConversationId, Content));
            MessagingMetrics.MessageUpdated.Add(1);
        }

        return this;
    }

    /// <summary>
    /// Adds an attachment to the message.
    /// </summary>
    public void AddAttachment(string fileUrl, string fileName, string fileType, long fileSize, string? thumbnailUrl = null)
    {
        var attachment = MessageAttachment.Create(Id, fileUrl, fileName, fileType, fileSize, thumbnailUrl);
        _attachments.Add(attachment);
        MessagingMetrics.AttachmentAdded.Add(1);
    }

    /// <summary>
    /// Soft deletes the message.
    /// </summary>
    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;

        QueueDomainEvent(new MessageDeleted(Id, ConversationId));
        MessagingMetrics.MessageDeleted.Add(1);
    }
}

/// <summary>
/// Defines the available message types.
/// </summary>
public static class MessageTypes
{
    public const string Text = "text";
    public const string File = "file";
    public const string Image = "image";
    public const string Video = "video";
    public const string Audio = "audio";
    public const string Document = "document";
    public const string System = "system";
}

