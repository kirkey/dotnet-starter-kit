using Asp.Versioning;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Warehouse.Features.Categories.Create.v1;

public static class CreateCategoryEndpoint
{
    internal static RouteHandlerBuilder MapCategoryCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateCategoryCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.CreatedAtRoute(nameof(CreateCategoryEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateCategoryEndpoint))
        .WithSummary("Creates a category")
        .WithDescription("Creates a category")
        .Produces<CreateCategoryResponse>(StatusCodes.Status201Created)
        .RequirePermission("Permissions.Warehouse.Create")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}
