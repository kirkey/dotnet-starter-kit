using Accounting.Application.FiscalPeriodCloses.Commands.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

/// <summary>
/// Endpoint for adding a validation issue to a fiscal period close.
/// </summary>
public static class AddFiscalPeriodCloseValidationIssueEndpoint
{
    internal static RouteHandlerBuilder MapAddFiscalPeriodCloseValidationIssueEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/validation-issues", async (DefaultIdType id, AddFiscalPeriodCloseValidationIssueCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var closeId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = closeId, Message = "Validation issue added successfully" });
            })
            .WithName(nameof(AddFiscalPeriodCloseValidationIssueEndpoint))
            .WithSummary("Add a validation issue to fiscal period close")
            .WithDescription("Adds a validation issue to the fiscal period close process")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
