using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TaxBrackets;

public static class DeleteTaxBracketEndpoint
{
    public static RouteHandlerBuilder MapDeleteTaxBracketEndpoint(this RouteGroupBuilder group)
    {
        return group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new DeleteTaxBracketCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("DeleteTaxBracket")
        .WithSummary("Delete tax bracket")
        .WithDescription("Deletes tax bracket by its unique identifier")
        .Produces<DeleteTaxBracketResponse>()
        .RequirePermission("Permissions.TaxBrackets.Delete")
        .MapToApiVersion(1);
    }
}
