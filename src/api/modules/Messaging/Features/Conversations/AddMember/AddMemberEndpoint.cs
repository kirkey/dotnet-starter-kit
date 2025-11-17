using Shared.Authorization;

namespace FSH.Starter.WebApi.Messaging.Features.Conversations.AddMember;

/// <summary>
/// Endpoint for adding a member to a conversation.
/// </summary>
public static class AddMemberEndpoint
{
    internal static RouteHandlerBuilder MapAddMemberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{conversationId:guid}/members", async (DefaultIdType conversationId, AddMemberCommand request, ISender mediator) =>
        {
            if (conversationId != request.ConversationId)
            {
                return Results.BadRequest("conversation id in route does not match id in request body");
            }

            await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok();
        })
        .WithName(nameof(AddMemberEndpoint))
        .WithSummary("adds a member to a conversation")
        .WithDescription("adds a member to a conversation")
        .Produces(StatusCodes.Status200OK)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Messaging))
        .MapToApiVersion(1);
    }
}

