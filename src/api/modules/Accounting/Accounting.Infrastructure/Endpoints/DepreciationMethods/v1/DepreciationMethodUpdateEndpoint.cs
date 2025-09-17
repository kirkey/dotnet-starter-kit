using Accounting.Application.DepreciationMethods.Update;

// Endpoint for updating a depreciation method
namespace Accounting.Infrastructure.Endpoints.DepreciationMethods.v1;

public static class DepreciationMethodUpdateEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationMethodUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateDepreciationMethodRequest request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DepreciationMethodUpdateEndpoint))
            .WithSummary("Update a depreciation method")
            .WithDescription("Updates an existing depreciation method")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
