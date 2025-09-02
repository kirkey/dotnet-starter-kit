using Accounting.Application.DepreciationMethods.Delete;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

// Endpoint for deleting a depreciation method
namespace Accounting.Infrastructure.Endpoints.DepreciationMethods.v1;

public static class DepreciationMethodDeleteEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationMethodDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteDepreciationMethodRequest(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(DepreciationMethodDeleteEndpoint))
            .WithSummary("Delete a depreciation method")
            .WithDescription("Deletes a depreciation method by its ID")
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
