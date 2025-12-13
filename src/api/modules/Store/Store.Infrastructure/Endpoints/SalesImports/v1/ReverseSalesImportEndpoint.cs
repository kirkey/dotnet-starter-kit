using FSH.Starter.WebApi.Store.Application.SalesImports.Reverse.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.SalesImports.v1;

/// <summary>
/// Endpoint for reversing a sales import.
/// </summary>
public static class ReverseSalesImportEndpoint
{
    internal static RouteHandlerBuilder MapReverseSalesImportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reverse", async (Guid id, ReverseSalesImportCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body");
                }

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ReverseSalesImportEndpoint))
            .WithSummary("Reverse a sales import")
            .WithDescription("Reverses a completed sales import by creating offsetting inventory transactions")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}

