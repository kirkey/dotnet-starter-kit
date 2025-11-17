using Accounting.Application.Patronages.Commands;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Patronage.v1;

public static class RetirePatronageEndpoint
{
    internal static RouteHandlerBuilder MapRetirePatronageEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/retire", async (RetirePatronageCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName(nameof(RetirePatronageEndpoint))
            .WithSummary("Retire patronage capital")
            .WithDescription("Process the retirement of patronage capital")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
