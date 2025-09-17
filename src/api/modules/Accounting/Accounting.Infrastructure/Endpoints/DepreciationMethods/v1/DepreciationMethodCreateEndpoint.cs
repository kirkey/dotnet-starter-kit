using Accounting.Application.DepreciationMethods.Create;

namespace Accounting.Infrastructure.Endpoints.DepreciationMethods.v1;

public static class DepreciationMethodCreateEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationMethodCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateDepreciationMethodRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DepreciationMethodCreateEndpoint))
            .WithSummary("Create a depreciation method")
            .WithDescription("Creates a new depreciation method")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

