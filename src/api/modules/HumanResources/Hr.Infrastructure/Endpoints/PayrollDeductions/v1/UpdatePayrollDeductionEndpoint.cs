using FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollDeductions.v1;

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
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

