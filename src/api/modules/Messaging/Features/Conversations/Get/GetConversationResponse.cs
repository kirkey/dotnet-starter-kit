namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Get;

/// <summary>
/// Response containing conversation details.
/// </summary>
public record GetConversationResponse(
    DefaultIdType Id,
    string? Title,
    string ConversationType,
    string? Description,
    string? ImageUrl,
    bool IsActive,
    DateTime? LastMessageAt,
    List<ConversationMemberDto> Members,
    DateTimeOffset CreatedOn);

/// <summary>
/// DTO for conversation member information.
/// </summary>
public record ConversationMemberDto(
    DefaultIdType UserId,
    string Role,
    bool IsActive,
    DateTime JoinedAt,
    DateTime? LastReadAt);

