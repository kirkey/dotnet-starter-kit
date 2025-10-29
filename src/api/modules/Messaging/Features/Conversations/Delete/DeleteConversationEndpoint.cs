namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Delete;

/// <summary>
/// Endpoint for deleting a conversation.
/// </summary>
public static class DeleteConversationEndpoint
{
    internal static RouteHandlerBuilder MapDeleteConversationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            await mediator.Send(new DeleteConversationCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(DeleteConversationEndpoint))
        .WithSummary("deletes a conversation")
        .WithDescription("soft deletes a conversation")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission("Permissions.Messaging.Delete")
        .MapToApiVersion(1);
    }
}
