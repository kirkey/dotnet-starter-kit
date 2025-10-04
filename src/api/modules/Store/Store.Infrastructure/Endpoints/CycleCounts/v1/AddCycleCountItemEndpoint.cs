using FSH.Starter.WebApi.Store.Application.CycleCounts.AddItem.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

public static class AddCycleCountItemEndpoint
{
    internal static RouteHandlerBuilder MapAddCycleCountItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/items", async (DefaultIdType id, AddCycleCountItemCommand command, ISender sender) =>
        {
            if (id != command.CycleCountId) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/cycle-counts/{result.CycleCountId}/items/{result.ItemId}", result);
        })
        .WithName(nameof(AddCycleCountItemEndpoint))
        .WithSummary("Add an item count to a cycle count")
        .WithDescription("Adds counted quantity for a grocery item to the cycle count")
        .Produces<AddCycleCountItemResponse>()
        .MapToApiVersion(1);
    }
}
