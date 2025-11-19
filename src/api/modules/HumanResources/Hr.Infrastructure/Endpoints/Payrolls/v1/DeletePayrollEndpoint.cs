using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls.v1;

/// <summary>
/// Endpoint for deleting a payroll period.
/// Only Draft payrolls can be deleted.
/// </summary>
public static class DeletePayrollEndpoint
{
    internal static RouteHandlerBuilder MapDeletePayrollEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new DeletePayrollCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeletePayrollEndpoint))
            .WithSummary("Deletes a payroll period")
            .WithDescription("Deletes a payroll period. Only Draft payrolls can be deleted. Processed or posted payrolls cannot be deleted.")
            .Produces<DeletePayrollResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

