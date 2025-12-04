using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Share Products.
/// </summary>
public static class ShareProductEndpoints
{
    /// <summary>
    /// Maps all Share Product endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapShareProductEndpoints(this IEndpointRouteBuilder app)
    {
        var shareProductsGroup = app.MapGroup("share-products").WithTags("share-products");

        shareProductsGroup.MapPost("/", async (CreateShareProductCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/share-products/{response.Id}", response);
            })
            .WithName("CreateShareProduct")
            .WithSummary("Creates a new share product")
            .Produces<CreateShareProductResponse>(StatusCodes.Status201Created);

        shareProductsGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetShareProductRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetShareProduct")
            .WithSummary("Gets a share product by ID")
            .Produces<ShareProductResponse>();

        shareProductsGroup.MapPost("/search", async (SearchShareProductsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchShareProducts")
            .WithSummary("Searches share products with filtering and pagination")
            .Produces<PagedList<ShareProductResponse>>();

        shareProductsGroup.MapPut("/{id:guid}", async (Guid id, UpdateShareProductCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateShareProduct")
            .WithSummary("Updates an existing share product")
            .Produces<UpdateShareProductResponse>();

        return app;
    }
}
