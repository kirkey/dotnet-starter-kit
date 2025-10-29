namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Get;

/// <summary>
/// Request for getting a conversation by ID.
/// </summary>
public record GetConversationRequest(DefaultIdType Id) : IRequest<GetConversationResponse>;
