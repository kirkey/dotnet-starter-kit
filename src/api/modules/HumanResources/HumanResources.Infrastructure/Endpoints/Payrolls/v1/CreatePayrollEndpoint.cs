using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls.v1;

/// <summary>
/// Endpoint for creating a new payroll period.
/// </summary>
public static class CreatePayrollEndpoint
{
    internal static RouteHandlerBuilder MapCreatePayrollEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePayrollCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetPayrollEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreatePayrollEndpoint))
            .WithSummary("Creates a new payroll period")
            .WithDescription("Creates a new payroll period for processing employee pay. Payroll is created in Draft status and must be processed before GL posting.")
            .Produces<CreatePayrollResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.Payrolls.Create")
            .MapToApiVersion(1);
    }
}

