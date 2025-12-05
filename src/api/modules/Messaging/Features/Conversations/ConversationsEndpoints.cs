using Carter;
using FSH.Starter.WebApi.Messaging.Features.Conversations.AddMember;
using FSH.Starter.WebApi.Messaging.Features.Conversations.AssignAdmin;
using FSH.Starter.WebApi.Messaging.Features.Conversations.Create;
using FSH.Starter.WebApi.Messaging.Features.Conversations.Get;
using FSH.Starter.WebApi.Messaging.Features.Conversations.GetList;
using FSH.Starter.WebApi.Messaging.Features.Conversations.MarkAsRead;
using FSH.Starter.WebApi.Messaging.Features.Conversations.RemoveMember;

namespace FSH.Starter.WebApi.Messaging.Features.Conversations;

/// <summary>
/// Endpoint configuration for Conversations module.
/// </summary>
public class ConversationsEndpoints() : CarterModule("messaging")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("conversations").WithTags("conversations");

        // Create conversation
        group.MapCreateConversationEndpoint();

        // Get conversation by ID
        group.MapGetConversationEndpoint();

        // Get conversation list
        group.MapGetConversationListEndpoint();

        // Add member to conversation
        group.MapAddMemberEndpoint();

        // Remove member from conversation
        group.MapRemoveMemberEndpoint();

        // Assign admin role to a member
        group.MapAssignAdminEndpoint();

        // Mark conversation as read
        group.MapMarkAsReadEndpoint();
    }
}
