namespace FSH.Starter.WebApi.Messaging.Features.Conversations.MarkAsRead;

/// <summary>
/// Endpoint for marking messages as read in a conversation.
/// </summary>
public static class MarkAsReadEndpoint
{
    internal static RouteHandlerBuilder MapMarkAsReadEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{conversationId:guid}/mark-read", async (DefaultIdType conversationId, ISender mediator) =>
        {
            await mediator.Send(new MarkAsReadCommand(conversationId)).ConfigureAwait(false);
            return Results.Ok();
        })
        .WithName(nameof(MarkAsReadEndpoint))
        .WithSummary("marks messages as read in a conversation")
        .WithDescription("updates the last read timestamp for the current user")
        .Produces(StatusCodes.Status200OK)
        .RequirePermission("Permissions.Messaging.Update")
        .MapToApiVersion(1);
    }
}

