using Shared.Authorization;

namespace FSH.Starter.WebApi.Messaging.Features.Conversations.RemoveMember;

/// <summary>
/// Endpoint for removing a member from a conversation.
/// </summary>
public static class RemoveMemberEndpoint
{
    internal static RouteHandlerBuilder MapRemoveMemberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{conversationId:guid}/members/{userId:guid}", async (DefaultIdType conversationId, DefaultIdType userId, ISender mediator) =>
        {
            await mediator.Send(new RemoveMemberCommand(conversationId, userId)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(RemoveMemberEndpoint))
        .WithSummary("removes a member from a conversation")
        .WithDescription("removes a member from a conversation")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Messaging))
        .MapToApiVersion(1);
    }
}

