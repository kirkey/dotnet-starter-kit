using FSH.Starter.WebApi.HumanResources.Application.Deductions.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Deductions.v1;

/// <summary>
/// Endpoint for updating an existing deduction type.
/// </summary>
public static class UpdateDeductionEndpoint
{
    internal static RouteHandlerBuilder MapUpdateDeductionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateDeductionCommand body, ISender mediator) =>
            {
                if (body.Id != id)
                    return Results.BadRequest(new { title = "ID mismatch." });

                var response = await mediator.Send(body).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateDeductionEndpoint))
            .WithSummary("Update Deduction Type")
            .WithDescription("Updates deduction type details, recovery rules, and compliance settings.")
            .Produces<UpdateDeductionResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

