using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollateralTypeEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collateral-types").WithTags("Collateral Types");

        group.MapPost("/", async (CreateCollateralTypeCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collateral-types/{result.Id}", result);
        })
        .WithName("CreateCollateralType")
        .WithSummary("Create a new collateral type")
        .Produces<CreateCollateralTypeResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollateralTypeRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetCollateralType")
        .WithSummary("Get collateral type by ID")
        .Produces<CollateralTypeResponse>();

    }
}
