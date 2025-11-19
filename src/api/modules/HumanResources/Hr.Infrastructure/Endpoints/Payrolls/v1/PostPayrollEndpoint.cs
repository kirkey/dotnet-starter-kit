using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Post.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls.v1;

/// <summary>
/// Endpoint for posting a payroll to the general ledger.
/// Transitions from Processed to Posted status and locks for editing.
/// </summary>
public static class PostPayrollEndpoint
{
    internal static RouteHandlerBuilder MapPostPayrollEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/post", async (DefaultIdType id, PostPayrollCommand command, ISender mediator) =>
            {
                var request = command with { Id = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PostPayrollEndpoint))
            .WithSummary("Posts a payroll to the general ledger")
            .WithDescription("Posts a processed payroll to the GL with the specified journal entry ID. Locks payroll from further editing. Transitions to Posted status.")
            .Produces<PostPayrollResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

