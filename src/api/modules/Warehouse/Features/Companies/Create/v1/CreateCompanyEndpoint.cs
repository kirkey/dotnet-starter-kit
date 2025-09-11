using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Companies.Create.v1;

public static class CreateCompanyEndpoint
{
    internal static RouteHandlerBuilder MapCompanyCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateCompanyCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(CreateCompanyEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateCompanyEndpoint))
        .WithSummary("Creates a company")
        .WithDescription("Creates a company")
        .Produces<CreateCompanyResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Warehouse.Create")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}
