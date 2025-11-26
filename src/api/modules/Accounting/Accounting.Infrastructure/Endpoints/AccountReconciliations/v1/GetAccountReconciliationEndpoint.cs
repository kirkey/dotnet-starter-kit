
using Accounting.Application.AccountReconciliations.Get.v1;
using Accounting.Application.AccountReconciliations.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliations.v1;

/// <summary>
/// Endpoint for retrieving an account reconciliation by ID.
/// </summary>
public static class GetAccountReconciliationEndpoint
{
    internal static RouteHandlerBuilder MapGetAccountReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAccountReconciliationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetAccountReconciliationEndpoint))
            .WithSummary("Get Account Reconciliation")
            .WithDescription("Get account reconciliation details by ID")
            .Produces<AccountReconciliationResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

