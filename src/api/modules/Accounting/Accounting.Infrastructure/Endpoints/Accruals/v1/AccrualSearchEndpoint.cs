using Accounting.Application.Accruals.Queries;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

// Endpoint for creating an accrual
namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class AccrualSearchEndpoint
{
    internal static RouteHandlerBuilder MapAccrualSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchAccrualsQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccrualSearchEndpoint))
            .WithSummary("Search accruals")
            .WithDescription("Searches accrual entries with filters and pagination")
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


