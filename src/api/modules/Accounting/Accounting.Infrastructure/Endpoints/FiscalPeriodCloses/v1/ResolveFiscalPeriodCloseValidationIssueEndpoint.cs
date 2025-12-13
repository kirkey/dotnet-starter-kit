using Accounting.Application.FiscalPeriodCloses.Commands.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

/// <summary>
/// Endpoint for resolving a validation issue in a fiscal period close.
/// </summary>
public static class ResolveFiscalPeriodCloseValidationIssueEndpoint
{
    internal static RouteHandlerBuilder MapResolveFiscalPeriodCloseValidationIssueEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}/validation-issues/resolve", async (DefaultIdType id, ResolveFiscalPeriodCloseValidationIssueCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var closeId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = closeId, Message = "Validation issue resolved successfully" });
            })
            .WithName(nameof(ResolveFiscalPeriodCloseValidationIssueEndpoint))
            .WithSummary("Resolve a fiscal period close validation issue")
            .WithDescription("Marks a validation issue as resolved in the fiscal period close process")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


