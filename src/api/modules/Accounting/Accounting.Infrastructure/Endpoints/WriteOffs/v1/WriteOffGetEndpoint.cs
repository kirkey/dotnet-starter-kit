using Accounting.Application.WriteOffs.Get;
using Accounting.Application.WriteOffs.Responses;

namespace Accounting.Infrastructure.Endpoints.WriteOffs.v1;

public static class WriteOffGetEndpoint
{
    internal static RouteHandlerBuilder MapWriteOffGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetWriteOffRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(WriteOffGetEndpoint))
            .WithSummary("Get write-off by ID")
            .WithDescription("Retrieves a write-off by its unique identifier")
            .Produces<WriteOffResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

