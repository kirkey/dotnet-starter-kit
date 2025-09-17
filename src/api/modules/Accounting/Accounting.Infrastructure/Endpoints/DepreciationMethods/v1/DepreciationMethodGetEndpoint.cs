using Accounting.Application.DepreciationMethods.Get;

// Endpoint for getting a depreciation method
namespace Accounting.Infrastructure.Endpoints.DepreciationMethods.v1;

public static class DepreciationMethodGetEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationMethodGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetDepreciationMethodRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DepreciationMethodGetEndpoint))
            .WithSummary("Get a depreciation method by ID")
            .WithDescription("Gets the details of a depreciation method by its ID")
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
