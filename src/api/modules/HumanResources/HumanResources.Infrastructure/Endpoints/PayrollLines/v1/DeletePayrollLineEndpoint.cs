using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollLines.v1;

/// <summary>
/// Endpoint for deleting a payroll line.
/// Only lines from draft payrolls can be deleted.
/// </summary>
public static class DeletePayrollLineEndpoint
{
    internal static RouteHandlerBuilder MapDeletePayrollLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new DeletePayrollLineCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeletePayrollLineEndpoint))
            .WithSummary("Deletes a payroll line")
            .WithDescription("Deletes a payroll line. Only lines from draft payrolls can be deleted. Processed payroll lines cannot be deleted.")
            .Produces<DeletePayrollLineResponse>()
            .RequirePermission("Permissions.PayrollLines.Delete")
            .MapToApiVersion(1);
    }
}

