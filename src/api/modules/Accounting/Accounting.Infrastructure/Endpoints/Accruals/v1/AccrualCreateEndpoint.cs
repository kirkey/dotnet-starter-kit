using Accounting.Application.Accruals.Commands;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class AccrualCreateEndpoint
{
    internal static RouteHandlerBuilder MapAccrualCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAccrualCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccrualCreateEndpoint))
            .WithSummary("Create an accrual")
            .WithDescription("Creates a new accrual entry")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

