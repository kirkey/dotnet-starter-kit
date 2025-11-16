using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.Payrolls.CompleteProcessing.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls.v1;

/// <summary>
/// Endpoint for completing payroll processing.
/// Transitions from Processing to Processed status.
/// </summary>
public static class CompletePayrollProcessingEndpoint
{
    internal static RouteHandlerBuilder MapCompletePayrollProcessingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/complete-processing", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new CompletePayrollProcessingCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CompletePayrollProcessingEndpoint))
            .WithSummary("Completes payroll processing")
            .WithDescription("Completes processing of a payroll period and transitions to Processed status. Ready for GL posting.")
            .Produces<CompletePayrollProcessingResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

