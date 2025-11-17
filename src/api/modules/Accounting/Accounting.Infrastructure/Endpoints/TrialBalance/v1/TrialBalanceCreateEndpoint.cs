using Accounting.Application.TrialBalance.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.TrialBalance.v1;

/// <summary>
/// Endpoint for creating a new trial balance report.
/// </summary>
public static class TrialBalanceCreateEndpoint
{
    internal static RouteHandlerBuilder MapTrialBalanceCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (TrialBalanceCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/trial-balance/{response.Id}", response);
            })
            .WithName(nameof(TrialBalanceCreateEndpoint))
            .WithSummary("Create a new trial balance report")
            .WithDescription("Creates a trial balance report and optionally auto-generates line items from General Ledger")
            .Produces<TrialBalanceCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
