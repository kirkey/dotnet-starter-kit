namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Create;

/// <summary>
/// Command for creating a new conversation.
/// </summary>
public record CreateConversationCommand(
    [property: DefaultValue("group")] string ConversationType,
    [property: DefaultValue("Team Discussion")] string? Title,
    [property: DefaultValue("General team discussion")] string? Description,
    [property: DefaultValue(null)] string? ImageUrl,
    [property: DefaultValue(new string[] { })] List<DefaultIdType>? MemberIds) : IRequest<CreateConversationResponse>;
