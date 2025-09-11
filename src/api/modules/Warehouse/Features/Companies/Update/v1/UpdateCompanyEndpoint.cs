using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Companies.Update.v1;

public static class UpdateCompanyEndpoint
{
    internal static RouteHandlerBuilder MapCompanyUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateCompanyCommand request, ISender mediator) =>
        {
            if (id != request.Id) return Results.BadRequest();
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateCompanyEndpoint))
        .WithSummary("Updates a company")
        .WithDescription("Updates a company")
        .Produces<UpdateCompanyResponse>(StatusCodes.Status200OK)
        .RequirePermission("Permissions.Warehouse.Update")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}

