using Accounting.Application.RetainedEarnings.UpdateNetIncome.v1;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

public static class RetainedEarningsUpdateNetIncomeEndpoint
{
    internal static RouteHandlerBuilder MapRetainedEarningsUpdateNetIncomeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/net-income", async (DefaultIdType id, UpdateNetIncomeCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var reId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = reId });
            })
            .WithName(nameof(RetainedEarningsUpdateNetIncomeEndpoint))
            .WithSummary("Update net income")
            .WithDescription("Updates the net income for the fiscal year")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

