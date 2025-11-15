using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollLines.v1;

/// <summary>
/// Endpoint for updating a payroll line.
/// Primarily used for adjusting hours and pay calculations before payroll is locked.
/// </summary>
public static class UpdatePayrollLineEndpoint
{
    internal static RouteHandlerBuilder MapUpdatePayrollLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdatePayrollLineCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePayrollLineEndpoint))
            .WithSummary("Updates a payroll line")
            .WithDescription("Updates a payroll line's hours and pay information. Only draft payroll lines can be updated.")
            .Produces<UpdatePayrollLineResponse>()
            .RequirePermission("Permissions.PayrollLines.Update")
            .MapToApiVersion(1);
    }
}

