using Accounting.Application.TrialBalance.Queries.GenerateTrialBalance.v1;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

// Endpoint for searching trial balances
namespace Accounting.Infrastructure.Endpoints.TrialBalance.v1;

public static class TrialBalanceSearchEndpoint
{
    internal static RouteHandlerBuilder MapTrialBalanceSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (GenerateTrialBalanceQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(TrialBalanceSearchEndpoint))
            .WithSummary("Search trial balances")
            .WithDescription("Generates a trial balance report for a given date and filters")
            .Produces<TrialBalanceDto>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
