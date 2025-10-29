namespace FSH.Starter.WebApi.Messaging.Domain;

/// <summary>
/// Represents a file attachment associated with a message.
/// </summary>
public sealed class MessageAttachment : AuditableEntity
{
    private MessageAttachment() { }

    private MessageAttachment(
        DefaultIdType messageId,
        string fileUrl,
        string fileName,
        string fileType,
        long fileSize,
        string? thumbnailUrl = null)
    {
        MessageId = messageId;
        FileUrl = fileUrl;
        FileName = fileName;
        FileType = fileType;
        FileSize = fileSize;
        ThumbnailUrl = thumbnailUrl;
        UploadedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets or sets the message ID this attachment belongs to.
    /// </summary>
    public DefaultIdType MessageId { get; private set; }

    /// <summary>
    /// Gets or sets the navigation property to the message.
    /// </summary>
    public Message Message { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the URL to the uploaded file.
    /// </summary>
    public string FileUrl { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the original file name.
    /// </summary>
    public string FileName { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the file type/extension.
    /// </summary>
    public string FileType { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the file size in bytes.
    /// </summary>
    public long FileSize { get; private set; }

    /// <summary>
    /// Gets or sets the optional thumbnail URL for images/videos.
    /// </summary>
    public string? ThumbnailUrl { get; private set; }

    /// <summary>
    /// Gets or sets when the file was uploaded.
    /// </summary>
    public DateTime UploadedAt { get; private set; }

    /// <summary>
    /// Creates a new message attachment.
    /// </summary>
    public static MessageAttachment Create(
        DefaultIdType messageId,
        string fileUrl,
        string fileName,
        string fileType,
        long fileSize,
        string? thumbnailUrl = null)
    {
        return new MessageAttachment(messageId, fileUrl, fileName, fileType, fileSize, thumbnailUrl);
    }
}

