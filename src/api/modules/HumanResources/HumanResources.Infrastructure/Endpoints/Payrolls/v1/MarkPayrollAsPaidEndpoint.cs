using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.MarkAsPaid.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls.v1;

/// <summary>
/// Endpoint for marking a payroll as paid.
/// Transitions from Posted to Paid status.
/// </summary>
public static class MarkPayrollAsPaidEndpoint
{
    internal static RouteHandlerBuilder MapMarkPayrollAsPaidEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/mark-as-paid", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new MarkPayrollAsPaidCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkPayrollAsPaidEndpoint))
            .WithSummary("Marks a payroll as paid")
            .WithDescription("Marks a posted payroll as paid. Records payment date and transitions to Paid status.")
            .Produces<MarkPayrollAsPaidResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.MarkAsPaid, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

