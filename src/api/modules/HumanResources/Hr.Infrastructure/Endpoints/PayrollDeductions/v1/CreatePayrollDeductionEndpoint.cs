using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollDeductions;

public static class CreatePayrollDeductionEndpoint
{
    internal static RouteHandlerBuilder MapCreatePayrollDeductionEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/", async (CreatePayrollDeductionCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(CreatePayrollDeductionEndpoint))
        .WithSummary("Create a new payroll deduction")
        .WithDescription("Creates a new payroll deduction configuration for employees per Philippine Labor Code")
        .Produces<CreatePayrollDeductionResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}

