using Accounting.Application.DeferredRevenues.Commands;

namespace Accounting.Infrastructure.Endpoints.DeferredRevenue.v1;

public static class DeferredRevenueCreateEndpoint
{
    internal static RouteHandlerBuilder MapDeferredRevenueCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateDeferredRevenueCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeferredRevenueCreateEndpoint))
            .WithSummary("Create deferred revenue")
            .WithDescription("Creates a new deferred revenue entry")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

