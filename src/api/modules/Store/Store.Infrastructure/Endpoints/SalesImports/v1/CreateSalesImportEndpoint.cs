using FSH.Starter.WebApi.Store.Application.SalesImports.Create.v1;

namespace Store.Infrastructure.Endpoints.SalesImports.v1;

/// <summary>
/// Endpoint for creating and processing a sales import from POS CSV data.
/// </summary>
public static class CreateSalesImportEndpoint
{
    internal static RouteHandlerBuilder MapCreateSalesImportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateSalesImportCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateSalesImportEndpoint))
            .WithSummary("Import POS sales data")
            .WithDescription("Creates and processes a sales import from POS CSV file to update inventory levels")
            .Produces<CreateSalesImportResponse>()
            .RequirePermission("Permissions.Store.Create")
            .MapToApiVersion(1);
    }
}

