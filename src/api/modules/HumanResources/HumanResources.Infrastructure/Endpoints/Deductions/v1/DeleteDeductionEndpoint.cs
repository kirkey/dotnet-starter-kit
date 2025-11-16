using FSH.Framework.Core.Domain;
using FSH.Starter.WebApi.HumanResources.Application.Deductions.Delete.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Deductions.v1;

/// <summary>
/// Endpoint for deleting a deduction type.
/// </summary>
public static class DeleteDeductionEndpoint
{
    internal static RouteHandlerBuilder MapDeleteDeductionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteDeductionCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return response.IsDeleted ? Results.Ok(response) : Results.NotFound();
            })
            .WithName(nameof(DeleteDeductionEndpoint))
            .WithSummary("Delete Deduction Type")
            .WithDescription("Deletes a deduction type from the master data.")
            .Produces<DeleteDeductionResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

