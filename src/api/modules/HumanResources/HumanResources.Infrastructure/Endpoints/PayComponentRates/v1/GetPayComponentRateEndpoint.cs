using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates.v1;

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
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}

