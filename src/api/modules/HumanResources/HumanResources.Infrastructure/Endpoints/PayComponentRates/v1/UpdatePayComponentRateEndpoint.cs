using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates.v1;

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
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}

