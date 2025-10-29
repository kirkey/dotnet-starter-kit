namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Delete;

/// <summary>
/// Command for deleting a conversation.
/// </summary>
public record DeleteConversationCommand(DefaultIdType Id) : IRequest;
