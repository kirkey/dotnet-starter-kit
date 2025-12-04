using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.UpdateNav.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class InvestmentProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/investment-products").WithTags("Investment Products");

        group.MapPost("/", async (CreateInvestmentProductCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/investment-products/{result.Id}", result);
        })
        .WithName("CreateInvestmentProduct")
        .WithSummary("Create a new investment product")
        .Produces<CreateInvestmentProductResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetInvestmentProductRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetInvestmentProduct")
        .WithSummary("Get investment product by ID")
        .Produces<InvestmentProductResponse>();

        group.MapPut("/{id:guid}/nav", async (Guid id, UpdateNavRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateInvestmentProductNavCommand(id, request.NewNav, request.NavDate));
            return Results.Ok(result);
        })
        .WithName("UpdateInvestmentProductNav")
        .WithSummary("Update investment product NAV")
        .Produces<UpdateInvestmentProductNavResponse>();

    }
}

public record UpdateNavRequest(decimal NewNav, DateOnly NavDate);
