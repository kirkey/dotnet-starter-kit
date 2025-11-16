using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponents;

public static class GetPayComponentEndpoint
{
    internal static RouteHandlerBuilder MapGetPayComponentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPayComponentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetPayComponentEndpoint))
            .WithSummary("Get a pay component by ID")
            .WithDescription("Retrieves a specific pay component by its unique identifier")
            .Produces<PayComponentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}
