using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponents;

public static class CreatePayComponentEndpoint
{
    internal static RouteHandlerBuilder MapCreatePayComponentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePayComponentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreatePayComponentEndpoint))
            .WithSummary("Create a new pay component")
            .WithDescription("Creates a new pay component for payroll calculation with Philippine labor law compliance")
            .Produces<CreatePayComponentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}
