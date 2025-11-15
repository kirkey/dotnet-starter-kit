using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollLines.v1;

/// <summary>
/// Endpoint for retrieving a specific payroll line by ID.
/// </summary>
public static class GetPayrollLineEndpoint
{
    internal static RouteHandlerBuilder MapGetPayrollLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetPayrollLineRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetPayrollLineEndpoint))
            .WithSummary("Gets a payroll line by ID")
            .WithDescription("Retrieves detailed information about a specific payroll line including hours, earnings, taxes, and deductions.")
            .Produces<PayrollLineResponse>()
            .RequirePermission("Permissions.PayrollLines.View")
            .MapToApiVersion(1);
    }
}

