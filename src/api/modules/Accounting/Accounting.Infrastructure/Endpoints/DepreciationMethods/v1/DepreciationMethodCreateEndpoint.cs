using Accounting.Application.DepreciationMethods.Create;
using Shared.Authorization;

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
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

