using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollDeductions;

public static class GetPayrollDeductionEndpoint
{
    internal static RouteHandlerBuilder MapGetPayrollDeductionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPayrollDeductionRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetPayrollDeductionEndpoint))
            .WithSummary("Get a payroll deduction by ID")
            .WithDescription("Retrieves a specific payroll deduction by its unique identifier")
            .Produces<PayrollDeductionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

