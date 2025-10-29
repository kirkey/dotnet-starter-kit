namespace FSH.Starter.WebApi.Messaging.Features.Messages.Get;

public record GetMessageRequest(DefaultIdType Id) : IRequest<GetMessageResponse>;

public record GetMessageResponse(
    DefaultIdType Id,
    DefaultIdType ConversationId,
    DefaultIdType SenderId,
    string Content,
    string MessageType,
    DefaultIdType? ReplyToMessageId,
    DateTime SentAt,
    bool IsEdited,
    DateTime? EditedAt,
    bool IsDeleted,
    List<AttachmentDto> Attachments);

public record AttachmentDto(
    DefaultIdType Id,
    string FileUrl,
    string FileName,
    string FileType,
    long FileSize,
    string? ThumbnailUrl);
