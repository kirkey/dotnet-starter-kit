using FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollDeductions;

public static class UpdatePayrollDeductionEndpoint
{
    internal static RouteHandlerBuilder MapUpdatePayrollDeductionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdatePayrollDeductionCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePayrollDeductionEndpoint))
            .WithSummary("Update a payroll deduction")
            .WithDescription("Updates an existing payroll deduction configuration")
            .Produces<UpdatePayrollDeductionResponse>()
            .RequirePermission("Permissions.PayrollDeductions.Update")
            .MapToApiVersion(1);
    }
}

