using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollateralTypeEndpoints() : CarterModule("microfinance")
{

    private const string CreateCollateralType = "CreateCollateralType";
    private const string GetCollateralType = "GetCollateralType";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collateral-types").WithTags("Collateral Types");

        group.MapPost("/", async (CreateCollateralTypeCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collateral-types/{result.Id}", result);
        })
        .WithName(CreateCollateralType)
        .WithSummary("Create a new collateral type")
        .Produces<CreateCollateralTypeResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollateralTypeRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCollateralType)
        .WithSummary("Get collateral type by ID")
        .Produces<CollateralTypeResponse>();

    }
}
