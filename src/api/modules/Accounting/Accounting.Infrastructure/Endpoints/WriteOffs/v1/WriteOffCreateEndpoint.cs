using Accounting.Application.WriteOffs.Create.v1;

namespace Accounting.Infrastructure.Endpoints.WriteOffs.v1;

public static class WriteOffCreateEndpoint
{
    internal static RouteHandlerBuilder MapWriteOffCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (WriteOffCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/write-offs/{response.Id}", response);
            })
            .WithName(nameof(WriteOffCreateEndpoint))
            .WithSummary("Create write-off")
            .Produces<WriteOffCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

