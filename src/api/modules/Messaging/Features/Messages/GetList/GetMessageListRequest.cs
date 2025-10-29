namespace FSH.Starter.WebApi.Messaging.Features.Messages.GetList;

public record GetMessageListRequest(DefaultIdType ConversationId, PaginationFilter Filter) : IRequest<PagedList<MessageDto>>;

public record MessageDto(
    DefaultIdType Id,
    DefaultIdType SenderId,
    string Content,
    string MessageType,
    DefaultIdType? ReplyToMessageId,
    DateTime SentAt,
    bool IsEdited,
    bool IsDeleted,
    int AttachmentCount);

