using FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollDeductions;

public static class DeletePayrollDeductionEndpoint
{
    internal static RouteHandlerBuilder MapDeletePayrollDeductionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeletePayrollDeductionCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeletePayrollDeductionEndpoint))
            .WithSummary("Delete a payroll deduction")
            .WithDescription("Deletes a payroll deduction by its unique identifier")
            .Produces<DeletePayrollDeductionResponse>()
            .RequirePermission("Permissions.PayrollDeductions.Delete")
            .MapToApiVersion(1);
    }
}

