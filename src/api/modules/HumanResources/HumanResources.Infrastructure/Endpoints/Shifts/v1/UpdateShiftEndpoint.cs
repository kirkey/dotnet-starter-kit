using FSH.Starter.WebApi.HumanResources.Application.Shifts.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts.v1;

public static class UpdateShiftEndpoint
{
    internal static RouteHandlerBuilder MapUpdateShiftEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateShiftCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateShiftEndpoint))
            .WithSummary("Updates a shift")
            .WithDescription("Updates shift information")
            .Produces<UpdateShiftResponse>()
            .RequirePermission("Permissions.Employees.Manage")
            .MapToApiVersion(1);
    }
}

