using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeEducations.v1;

/// <summary>
/// Endpoint for updating employee education record.
/// </summary>
public static class UpdateEmployeeEducationEndpoint
{
    internal static RouteHandlerBuilder MapUpdateEmployeeEducationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateEmployeeEducationCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateEmployeeEducationEndpoint))
            .WithSummary("Updates employee education record")
            .WithDescription("Updates education details for an existing employee education record")
            .Produces<UpdateEmployeeEducationResponse>()
            .RequirePermission("Permissions.EmployeeEducations.Update")
            .MapToApiVersion(1);
    }
}

