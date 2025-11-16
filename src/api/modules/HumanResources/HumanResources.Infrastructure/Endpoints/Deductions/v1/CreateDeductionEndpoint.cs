using FSH.Framework.Core.Domain;
using FSH.Starter.WebApi.HumanResources.Application.Deductions.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Deductions.v1;

/// <summary>
/// Endpoint for creating a new deduction type.
/// </summary>
public static class CreateDeductionEndpoint
{
    internal static RouteHandlerBuilder MapCreateDeductionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateDeductionCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetDeductionEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateDeductionEndpoint))
            .WithSummary("Create Deduction Type")
            .WithDescription("Creates a new deduction type (loan, cash advance, uniform, etc) with recovery rules per Philippines Labor Code Art 113.")
            .Produces<CreateDeductionResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

