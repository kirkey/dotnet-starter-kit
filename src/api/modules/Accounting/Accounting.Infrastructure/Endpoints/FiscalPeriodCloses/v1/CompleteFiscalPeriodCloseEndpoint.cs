using Accounting.Application.FiscalPeriodCloses.Commands.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FiscalPeriodCloses.v1;

public static class CompleteFiscalPeriodCloseEndpoint
{
    internal static RouteHandlerBuilder MapCompleteFiscalPeriodCloseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/complete", async (DefaultIdType id, CompleteFiscalPeriodCloseCommand command, ISender mediator) =>
            {
                if (id != command.FiscalPeriodCloseId)
                    return Results.BadRequest("ID mismatch");
                    
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(CompleteFiscalPeriodCloseEndpoint))
            .WithSummary("Complete fiscal period close")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

