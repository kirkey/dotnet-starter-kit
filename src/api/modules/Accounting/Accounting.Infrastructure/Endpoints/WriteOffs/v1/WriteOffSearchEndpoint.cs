using Accounting.Application.WriteOffs.Responses;
using Accounting.Application.WriteOffs.Search.v1;

namespace Accounting.Infrastructure.Endpoints.WriteOffs.v1;

public static class WriteOffSearchEndpoint
{
    internal static RouteHandlerBuilder MapWriteOffSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchWriteOffsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(WriteOffSearchEndpoint))
            .WithSummary("Search write-offs")
            .Produces<List<WriteOffResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


