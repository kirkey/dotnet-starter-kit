using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Create.v1;
using MediatR;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates;

public static class CreatePayComponentRateEndpoint
{
    internal static RouteHandlerBuilder MapCreatePayComponentRateEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/", async (CreatePayComponentRateCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(CreatePayComponentRateEndpoint))
        .WithSummary("Create a new pay component rate")
        .WithDescription("Creates a new rate/bracket for pay component")
        .Produces<CreatePayComponentRateResponse>()
        .RequirePermission("Permissions.PayComponentRates.Create")
        .MapToApiVersion(1);
    }
}

