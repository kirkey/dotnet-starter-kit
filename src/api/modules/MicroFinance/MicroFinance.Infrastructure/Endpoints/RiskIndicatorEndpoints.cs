using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.RecordMeasurement.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class RiskIndicatorEndpoints() : CarterModule("microfinance")
{

    private const string ActivateRiskIndicator = "ActivateRiskIndicator";
    private const string CreateRiskIndicator = "CreateRiskIndicator";
    private const string DeactivateRiskIndicator = "DeactivateRiskIndicator";
    private const string GetRiskIndicator = "GetRiskIndicator";
    private const string RecordRiskIndicatorMeasurement = "RecordRiskIndicatorMeasurement";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/risk-indicators").WithTags("Risk Indicators");

        group.MapPost("/", async (CreateRiskIndicatorCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/risk-indicators/{result.Id}", result);
        })
        .WithName(CreateRiskIndicator)
        .WithSummary("Create a new risk indicator")
        .Produces<CreateRiskIndicatorResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetRiskIndicatorRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetRiskIndicator)
        .WithSummary("Get risk indicator by ID")
        .Produces<RiskIndicatorResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/measure", async (Guid id, RecordMeasurementRequest request, ISender sender) =>
        {
            var command = new RecordMeasurementCommand(id, request.Value);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(RecordRiskIndicatorMeasurement)
        .WithSummary("Record a new measurement for risk indicator")
        .Produces<RecordMeasurementResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateRiskIndicatorCommand(id));
            return Results.Ok(result);
        })
        .WithName(ActivateRiskIndicator)
        .WithSummary("Activate risk indicator")
        .Produces<ActivateRiskIndicatorResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/deactivate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateRiskIndicatorCommand(id));
            return Results.Ok(result);
        })
        .WithName(DeactivateRiskIndicator)
        .WithSummary("Deactivate risk indicator")
        .Produces<DeactivateRiskIndicatorResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public sealed record RecordMeasurementRequest(decimal Value);
