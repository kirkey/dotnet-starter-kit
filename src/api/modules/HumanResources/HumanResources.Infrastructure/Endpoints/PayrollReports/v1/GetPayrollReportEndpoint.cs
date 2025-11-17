namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Get.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for retrieving a payroll report by ID.
/// </summary>
public static class GetPayrollReportEndpoint
{
    /// <summary>
    /// Maps the get payroll report endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGetPayrollReportEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetPayrollReportRequest(id);
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetPayrollReportEndpoint))
        .WithSummary("Get payroll report")
        .WithDescription("Retrieves a payroll report by ID with all details including totals and averages")
        .Produces<PayrollReportResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}

