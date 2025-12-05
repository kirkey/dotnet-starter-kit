using Accounting.Application.Accruals.Update;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class UpdateAccrualEndpoint
{
    internal static RouteHandlerBuilder MapUpdateAccrualEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateAccrualCommand request, ISender mediator) =>
        {
            request.Id = id;
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateAccrualEndpoint))
        .WithSummary("Update an accrual")
        .WithDescription("Updates an accrual's mutable fields")
        .Produces<DefaultIdType>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}
