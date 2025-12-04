using Carter;
using FSH.Starter.WebApi.Store.Application.Categories.Create.v1;
using FSH.Starter.WebApi.Store.Application.Categories.Delete.v1;
using FSH.Starter.WebApi.Store.Application.Categories.Get.v1;
using FSH.Starter.WebApi.Store.Application.Categories.Search.v1;
using FSH.Starter.WebApi.Store.Application.Categories.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Categories;

/// <summary>
/// Endpoint configuration for Categories module.
/// </summary>
public class CategoriesEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Categories endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/categories").WithTags("categories");

        group.MapPost("/", async (CreateCategoryCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/store/categories/{response.Id}", response);
            })
            .WithName("CreateCategory")
            .WithSummary("Create a new category")
            .WithDescription("Creates a new product category")
            .Produces<CreateCategoryResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCategoryCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetCategory")
            .WithSummary("Get category by ID")
            .WithDescription("Retrieves a specific category by its ID")
            .Produces<CategoryResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateCategoryCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateCategory")
            .WithSummary("Update an existing category")
            .WithDescription("Updates an existing product category")
            .Produces<UpdateCategoryResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteCategoryCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteCategory")
            .WithSummary("Delete a category")
            .WithDescription("Deletes a product category")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCategoriesCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchCategories")
            .WithSummary("Search categories")
            .WithDescription("Searches for product categories with pagination and filtering")
            .Produces<PagedList<CategoryResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);
    }
}
