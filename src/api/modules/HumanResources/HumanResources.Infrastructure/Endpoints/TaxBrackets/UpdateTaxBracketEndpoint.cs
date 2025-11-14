using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TaxBrackets;

public static class UpdateTaxBracketEndpoint
{
    public static RouteHandlerBuilder MapUpdateTaxBracketEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateTaxBracketCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("UpdateTaxBracket")
        .WithSummary("Update tax bracket")
        .WithDescription("Updates tax bracket details")
        .Produces<UpdateTaxBracketResponse>()
        .RequirePermission("Permissions.TaxBrackets.Update")
        .MapToApiVersion(1);
    }
}
