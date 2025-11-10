using Accounting.Application.Consumptions.Create.v1;

namespace Accounting.Infrastructure.Endpoints.Consumptions.v1;

/// <summary>
/// Endpoint for creating a new consumption record.
/// </summary>
public static class ConsumptionCreateEndpoint
{
    internal static RouteGroupBuilder MapConsumptionCreateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (CreateConsumptionCommand command, ISender mediator) =>
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(ConsumptionCreateEndpoint))
        .WithSummary("Create consumption record")
        .WithDescription("Creates a new consumption/meter reading record")
        .RequirePermission("Permissions.Accounting.Create")
        .MapToApiVersion(1);

        return group;
    }
}

