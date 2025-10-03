using FSH.Starter.WebApi.Store.Application.LotNumbers.Create.v1;

namespace Store.Infrastructure.Endpoints.LotNumbers.v1;

public static class CreateLotNumberEndpoint
{
    internal static RouteHandlerBuilder MapCreateLotNumberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateLotNumberCommand request, ISender sender) =>
            {
                var response = await sender.Send(request);
                return Results.Created($"/api/v1/store/lotnumbers/{response.Id}", response);
            })
            .WithName(nameof(CreateLotNumberEndpoint))
            .WithSummary("Create a new lot number")
            .WithDescription("Creates a new lot/batch number for inventory traceability and expiration management")
            .RequirePermission("Permissions.Store.Create")
            .Produces<CreateLotNumberResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .MapToApiVersion(1);
    }
}
