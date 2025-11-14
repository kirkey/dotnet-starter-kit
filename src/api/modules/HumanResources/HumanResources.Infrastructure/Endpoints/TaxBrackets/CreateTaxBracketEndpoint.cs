using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Create.v1;

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
        .RequirePermission("Permissions.TaxBrackets.Create")
        .MapToApiVersion(1);
    }
}
