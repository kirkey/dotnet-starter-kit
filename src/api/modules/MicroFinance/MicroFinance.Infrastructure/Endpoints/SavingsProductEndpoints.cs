using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Savings Products.
/// </summary>
public static class SavingsProductEndpoints
{
    /// <summary>
    /// Maps all Savings Product endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapSavingsProductEndpoints(this IEndpointRouteBuilder app)
    {
        var savingsProductsGroup = app.MapGroup("savings-products").WithTags("savings-products");

        savingsProductsGroup.MapPost("/", async (CreateSavingsProductCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/savings-products/{response.Id}", response);
            })
            .WithName("CreateSavingsProduct")
            .WithSummary("Creates a new savings product")
            .Produces<CreateSavingsProductResponse>(StatusCodes.Status201Created);

        savingsProductsGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetSavingsProductRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetSavingsProduct")
            .WithSummary("Gets a savings product by ID")
            .Produces<SavingsProductResponse>();

        return app;
    }
}
