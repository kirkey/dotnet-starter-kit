namespace FSH.Starter.WebApi.Messaging.Features.Conversations.AddMember;

/// <summary>
/// Command for adding a member to a conversation.
/// </summary>
public record AddMemberCommand(
    DefaultIdType ConversationId,
    DefaultIdType UserId,
    [property: DefaultValue("member")] string Role = "member") : IRequest;
