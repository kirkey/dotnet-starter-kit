using Accounting.Application.FiscalPeriodCloses.Commands.v1;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

/// <summary>
/// Endpoint for adding a validation issue to a fiscal period close.
/// </summary>
public static class AddValidationIssueEndpoint
{
    internal static RouteHandlerBuilder MapAddValidationIssueEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/validation-issues", async (DefaultIdType id, AddValidationIssueCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var closeId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = closeId, Message = "Validation issue added successfully" });
            })
            .WithName(nameof(AddValidationIssueEndpoint))
            .WithSummary("Add a validation issue")
            .WithDescription("Adds a validation issue to the fiscal period close process")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
