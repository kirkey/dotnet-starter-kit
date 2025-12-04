using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.RecordMeasurement.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class RiskIndicatorEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/risk-indicators").WithTags("Risk Indicators");

        group.MapPost("/", async (CreateRiskIndicatorCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/risk-indicators/{result.Id}", result);
        })
        .WithName("CreateRiskIndicator")
        .WithSummary("Create a new risk indicator")
        .Produces<CreateRiskIndicatorResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetRiskIndicatorRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetRiskIndicator")
        .WithSummary("Get risk indicator by ID")
        .Produces<RiskIndicatorResponse>();

        group.MapPost("/{id:guid}/measure", async (Guid id, RecordMeasurementRequest request, ISender sender) =>
        {
            var command = new RecordMeasurementCommand(id, request.Value);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("RecordRiskIndicatorMeasurement")
        .WithSummary("Record a new measurement for risk indicator")
        .Produces<RecordMeasurementResponse>();

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ActivateRiskIndicatorCommand(id));
            return Results.Ok(result);
        })
        .WithName("ActivateRiskIndicator")
        .WithSummary("Activate risk indicator")
        .Produces<ActivateRiskIndicatorResponse>();

        group.MapPost("/{id:guid}/deactivate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeactivateRiskIndicatorCommand(id));
            return Results.Ok(result);
        })
        .WithName("DeactivateRiskIndicator")
        .WithSummary("Deactivate risk indicator")
        .Produces<DeactivateRiskIndicatorResponse>();

    }
}

public sealed record RecordMeasurementRequest(decimal Value);
