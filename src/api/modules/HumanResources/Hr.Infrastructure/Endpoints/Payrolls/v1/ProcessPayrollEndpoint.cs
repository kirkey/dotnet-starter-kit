using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Process.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls.v1;

/// <summary>
/// Endpoint for processing a payroll period.
/// Transitions from Draft to Processing status and initiates pay calculations.
/// </summary>
public static class ProcessPayrollEndpoint
{
    internal static RouteHandlerBuilder MapProcessPayrollEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/process", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new ProcessPayrollCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Accepted($"/payrolls/{response.Id}", response);
            })
            .WithName(nameof(ProcessPayrollEndpoint))
            .WithSummary("Processes a payroll period")
            .WithDescription("Initiates processing of a Draft payroll period. Calculates pay totals for all lines. Transitions to Processing status.")
            .Produces<ProcessPayrollResponse>(StatusCodes.Status202Accepted)
            .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

