namespace FSH.Starter.WebApi.Messaging.Features.Conversations.RemoveMember;

/// <summary>
/// Command for removing a member from a conversation.
/// </summary>
public record RemoveMemberCommand(
    DefaultIdType ConversationId,
    DefaultIdType UserId) : IRequest;

