using Accounting.Application.TaxCodes.Update.v1;

namespace Accounting.Infrastructure.Endpoints.TaxCodes.v1;

/// <summary>
/// Endpoint for updating tax code information.
/// </summary>
public static class TaxCodeUpdateEndpoint
{
    /// <summary>
    /// Maps the tax code update endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapTaxCodeUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateTaxCodeCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest();
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(TaxCodeUpdateEndpoint))
            .WithSummary("Update a tax code")
            .WithDescription("Update tax code information (non-rate fields)")
            .Produces<UpdateTaxCodeResponse>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

