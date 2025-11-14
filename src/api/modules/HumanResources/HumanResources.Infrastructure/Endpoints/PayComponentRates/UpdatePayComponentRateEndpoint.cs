using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Update.v1;
using MediatR;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates;

public static class UpdatePayComponentRateEndpoint
{
    internal static RouteHandlerBuilder MapUpdatePayComponentRateEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePayComponentRateCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdatePayComponentRateEndpoint))
        .WithSummary("Update a pay component rate")
        .WithDescription("Updates an existing pay component rate/bracket")
        .Produces<UpdatePayComponentRateResponse>()
        .RequirePermission("Permissions.PayComponentRates.Update")
        .MapToApiVersion(1);
    }
}

