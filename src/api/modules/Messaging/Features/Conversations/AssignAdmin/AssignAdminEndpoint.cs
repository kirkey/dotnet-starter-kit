namespace FSH.Starter.WebApi.Messaging.Features.Conversations.AssignAdmin;

/// <summary>
/// Endpoint for assigning admin role to a member.
/// </summary>
public static class AssignAdminEndpoint
{
    internal static RouteHandlerBuilder MapAssignAdminEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPatch("/{conversationId:guid}/members/{userId:guid}/role", async (DefaultIdType conversationId, DefaultIdType userId, AssignAdminCommand request, ISender mediator) =>
        {
            if (conversationId != request.ConversationId || userId != request.UserId)
            {
                return Results.BadRequest("ids in route do not match ids in request body");
            }

            await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok();
        })
        .WithName(nameof(AssignAdminEndpoint))
        .WithSummary("updates a member's role in a conversation")
        .WithDescription("assigns or revokes admin role for a member")
        .Produces(StatusCodes.Status200OK)
        .RequirePermission("Permissions.Messaging.Update")
        .MapToApiVersion(1);
    }
}

