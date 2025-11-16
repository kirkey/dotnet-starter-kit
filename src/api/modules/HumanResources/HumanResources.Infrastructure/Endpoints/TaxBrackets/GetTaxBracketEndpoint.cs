using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TaxBrackets;

public static class GetTaxBracketEndpoint
{
    public static RouteHandlerBuilder MapGetTaxBracketEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetTaxBracketRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetTaxBracket")
        .WithSummary("Get tax bracket by ID")
        .WithDescription("Retrieves tax bracket by its unique identifier")
        .Produces<TaxBracketResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}
