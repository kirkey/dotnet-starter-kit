using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollLines.v1;

/// <summary>
/// Endpoint for creating a new payroll line.
/// </summary>
public static class CreatePayrollLineEndpoint
{
    internal static RouteHandlerBuilder MapCreatePayrollLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePayrollLineCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetPayrollLineEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreatePayrollLineEndpoint))
            .WithSummary("Creates a new payroll line")
            .WithDescription("Creates a new payroll line for an employee within a payroll period. Contains hours worked and pay calculations.")
            .Produces<CreatePayrollLineResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

