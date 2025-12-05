using Accounting.Application.AccountReconciliations.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliations.v1;

/// <summary>
/// Endpoint for creating an account reconciliation.
/// </summary>
public static class CreateAccountReconciliationEndpoint
{
    internal static RouteHandlerBuilder MapCreateAccountReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAccountReconciliationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/account-reconciliations/{response}", null);
            })
            .WithName(nameof(CreateAccountReconciliationEndpoint))
            .WithSummary("Create Account Reconciliation")
            .WithDescription("Create a new account reconciliation comparing GL balance with subsidiary ledger")
            .Produces<DefaultIdType>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

