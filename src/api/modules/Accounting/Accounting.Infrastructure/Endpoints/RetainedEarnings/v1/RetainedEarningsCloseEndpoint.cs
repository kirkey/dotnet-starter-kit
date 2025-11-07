using Accounting.Application.RetainedEarnings.Close.v1;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

public static class RetainedEarningsCloseEndpoint
{
    internal static RouteHandlerBuilder MapRetainedEarningsCloseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/close", async (DefaultIdType id, CloseRetainedEarningsCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var reId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = reId, Message = "Fiscal year closed successfully" });
            })
            .WithName(nameof(RetainedEarningsCloseEndpoint))
            .WithSummary("Close fiscal year")
            .WithDescription("Closes the fiscal year for retained earnings")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

