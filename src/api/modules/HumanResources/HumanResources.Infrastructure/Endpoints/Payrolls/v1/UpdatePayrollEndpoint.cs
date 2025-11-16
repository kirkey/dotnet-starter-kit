using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls.v1;

/// <summary>
/// Endpoint for updating a payroll period.
/// Primarily used for administrative updates and notes.
/// </summary>
public static class UpdatePayrollEndpoint
{
    internal static RouteHandlerBuilder MapUpdatePayrollEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdatePayrollCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePayrollEndpoint))
            .WithSummary("Updates a payroll period")
            .WithDescription("Updates payroll period details such as notes. Locked payrolls cannot be updated.")
            .Produces<UpdatePayrollResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

