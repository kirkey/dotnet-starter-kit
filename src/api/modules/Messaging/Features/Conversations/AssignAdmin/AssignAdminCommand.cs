namespace FSH.Starter.WebApi.Messaging.Features.Conversations.AssignAdmin;

/// <summary>
/// Command for assigning or revoking admin role for a member.
/// </summary>
public record AssignAdminCommand(
    DefaultIdType ConversationId,
    DefaultIdType UserId,
    [property: DefaultValue("admin")] string Role) : IRequest;

