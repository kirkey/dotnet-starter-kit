using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls.v1;

/// <summary>
/// Endpoint for retrieving a specific payroll period by ID.
/// </summary>
public static class GetPayrollEndpoint
{
    internal static RouteHandlerBuilder MapGetPayrollEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetPayrollRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetPayrollEndpoint))
            .WithSummary("Gets a payroll period by ID")
            .WithDescription("Retrieves detailed information about a specific payroll period including totals, status, and GL posting details.")
            .Produces<PayrollResponse>(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Payrolls.View")
            .MapToApiVersion(1);
    }
}

