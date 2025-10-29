namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Update;

/// <summary>
/// Command for updating a conversation.
/// </summary>
public record UpdateConversationCommand(
    DefaultIdType Id,
    [property: DefaultValue("Updated Title")] string? Title,
    [property: DefaultValue("Updated description")] string? Description,
    [property: DefaultValue(null)] string? ImageUrl) : IRequest<UpdateConversationResponse>;
