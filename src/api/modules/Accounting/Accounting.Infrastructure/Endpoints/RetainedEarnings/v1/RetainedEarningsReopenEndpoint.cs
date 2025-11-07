using Accounting.Application.RetainedEarnings.Reopen.v1;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

public static class RetainedEarningsReopenEndpoint
{
    internal static RouteHandlerBuilder MapRetainedEarningsReopenEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reopen", async (DefaultIdType id, ReopenRetainedEarningsCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var reId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = reId, Message = "Fiscal year reopened successfully" });
            })
            .WithName(nameof(RetainedEarningsReopenEndpoint))
            .WithSummary("Reopen fiscal year")
            .WithDescription("Reopens a closed fiscal year for retained earnings")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

