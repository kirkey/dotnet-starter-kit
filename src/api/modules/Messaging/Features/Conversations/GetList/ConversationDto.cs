namespace FSH.Starter.WebApi.Messaging.Features.Conversations.GetList;
/// <summary>
/// DTO for conversation in list view.
/// </summary>
public record ConversationDto(
    DefaultIdType Id,
    string? Title,
    string ConversationType,
    string? ImageUrl,
    DateTime? LastMessageAt,
    int UnreadCount,
    string? LastMessageContent,
    DateTimeOffset CreatedOn);
