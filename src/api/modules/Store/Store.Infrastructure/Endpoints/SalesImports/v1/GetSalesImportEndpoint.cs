using FSH.Starter.WebApi.Store.Application.SalesImports.Get.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.SalesImports.v1;

/// <summary>
/// Endpoint for retrieving detailed sales import information.
/// </summary>
public static class GetSalesImportEndpoint
{
    internal static RouteHandlerBuilder MapGetSalesImportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetSalesImportRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetSalesImportEndpoint))
            .WithSummary("Get sales import details")
            .WithDescription("Retrieves detailed information about a sales import including all items")
            .Produces<SalesImportDetailResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);
    }
}

