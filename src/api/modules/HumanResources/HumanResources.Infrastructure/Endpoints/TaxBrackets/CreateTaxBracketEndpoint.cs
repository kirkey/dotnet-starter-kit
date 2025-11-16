using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TaxBrackets;

public static class CreateTaxBracketEndpoint
{
    public static RouteHandlerBuilder MapCreateTaxBracketEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/", async (CreateTaxBracketCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreateTaxBracket")
        .WithSummary("Create tax bracket")
        .WithDescription("Creates new tax bracket for income taxation")
        .Produces<CreateTaxBracketResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}
