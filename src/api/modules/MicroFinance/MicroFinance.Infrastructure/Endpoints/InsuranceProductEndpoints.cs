using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class InsuranceProductEndpoints() : CarterModule("microfinance")
{

    private const string ActivateInsuranceProduct = "ActivateInsuranceProduct";
    private const string CreateInsuranceProduct = "CreateInsuranceProduct";
    private const string GetInsuranceProduct = "GetInsuranceProduct";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/insurance-products").WithTags("insurance-products");

        group.MapPost("/", async (CreateInsuranceProductCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/insurance-products/{response.Id}", response);
            })
            .WithName(CreateInsuranceProduct)
            .WithSummary("Creates a new insurance product")
            .Produces<CreateInsuranceProductResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetInsuranceProductRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetInsuranceProduct)
            .WithSummary("Gets an insurance product by ID")
            .Produces<InsuranceProductResponse>();

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new ActivateInsuranceProductCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ActivateInsuranceProduct)
            .WithSummary("Activates an insurance product")
            .Produces<ActivateInsuranceProductResponse>();

    }
}
