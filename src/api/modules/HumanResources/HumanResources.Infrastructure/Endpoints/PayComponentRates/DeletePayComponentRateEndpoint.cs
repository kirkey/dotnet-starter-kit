using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Delete.v1;
using MediatR;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates;

public static class DeletePayComponentRateEndpoint
{
    internal static RouteHandlerBuilder MapDeletePayComponentRateEndpoint(this RouteGroupBuilder group)
    {
        return group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new DeletePayComponentRateCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(DeletePayComponentRateEndpoint))
        .WithSummary("Delete a pay component rate")
        .WithDescription("Deletes a pay component rate/bracket by its unique identifier")
        .Produces<DeletePayComponentRateResponse>()
        .RequirePermission("Permissions.PayComponentRates.Delete")
        .MapToApiVersion(1);
    }
}

