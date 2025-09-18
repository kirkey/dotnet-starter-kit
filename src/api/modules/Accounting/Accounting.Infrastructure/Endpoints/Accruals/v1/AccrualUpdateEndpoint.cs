using Accounting.Application.Accruals.Commands;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class AccrualUpdateEndpoint
{
    internal static RouteHandlerBuilder MapAccrualUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}/reverse", async (DefaultIdType id, ReverseAccrualCommand command, ISender mediator) =>
            {
                command.Id = id;
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(AccrualUpdateEndpoint))
            .WithSummary("Reverse an accrual")
            .WithDescription("Reverses an accrual entry by ID")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
