using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponents;

public static class UpdatePayComponentEndpoint
{
    internal static RouteHandlerBuilder MapUpdatePayComponentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdatePayComponentCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePayComponentEndpoint))
            .WithSummary("Update a pay component")
            .WithDescription("Updates an existing pay component")
            .Produces<UpdatePayComponentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}
