using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Savings Products.
/// </summary>
public class SavingsProductEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Savings Product endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var savingsProductsGroup = app.MapGroup("microfinance/savings-products").WithTags("savings-products");

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

        savingsProductsGroup.MapPost("/search", async (SearchSavingsProductsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchSavingsProducts")
            .WithSummary("Searches savings products with filters and pagination")
            .Produces<PagedList<SavingsProductResponse>>();

        savingsProductsGroup.MapPut("/{id:guid}", async (Guid id, UpdateSavingsProductCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateSavingsProduct")
            .WithSummary("Updates a savings product")
            .Produces<UpdateSavingsProductResponse>();
    }
}
