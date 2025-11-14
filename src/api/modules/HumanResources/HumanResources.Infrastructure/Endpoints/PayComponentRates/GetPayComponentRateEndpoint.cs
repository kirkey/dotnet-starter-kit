using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates;

public static class GetPayComponentRateEndpoint
{
    internal static RouteHandlerBuilder MapGetPayComponentRateEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetPayComponentRateRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetPayComponentRateEndpoint))
        .WithSummary("Get a pay component rate by ID")
        .WithDescription("Retrieves a specific pay component rate/bracket by its unique identifier")
        .Produces<PayComponentRateResponse>()
        .RequirePermission("Permissions.PayComponentRates.View")
        .MapToApiVersion(1);
    }
}

