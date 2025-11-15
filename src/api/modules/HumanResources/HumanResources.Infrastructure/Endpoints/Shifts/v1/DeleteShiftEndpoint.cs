using FSH.Starter.WebApi.HumanResources.Application.Shifts.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts.v1;

public static class DeleteShiftEndpoint
{
    internal static RouteHandlerBuilder MapDeleteShiftEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteShiftCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteShiftEndpoint))
            .WithSummary("Deletes a shift")
            .WithDescription("Deletes a shift template")
            .Produces<DeleteShiftResponse>()
            .RequirePermission("Permissions.Employees.Manage")
            .MapToApiVersion(1);
    }
}

