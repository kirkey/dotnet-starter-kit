using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Acknowledge.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Assign.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Escalate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Resolve.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class RiskAlertEndpoints() : CarterModule("microfinance")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/risk-alerts").WithTags("Risk Alerts");

        group.MapPost("/", async (CreateRiskAlertCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/risk-alerts/{result.Id}", result);
        })
        .WithName("CreateRiskAlert")
        .WithSummary("Create a new risk alert")
        .Produces<CreateRiskAlertResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetRiskAlertRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetRiskAlert")
        .WithSummary("Get risk alert by ID")
        .Produces<RiskAlertResponse>();

        group.MapPost("/{id:guid}/acknowledge", async (Guid id, AcknowledgeRiskAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AcknowledgeRiskAlertCommand(id, request.UserId));
            return Results.Ok(result);
        })
        .WithName("AcknowledgeRiskAlert")
        .WithSummary("Acknowledge a risk alert")
        .Produces<AcknowledgeRiskAlertResponse>();

        group.MapPost("/{id:guid}/assign", async (Guid id, AssignRiskAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AssignRiskAlertCommand(id, request.UserId));
            return Results.Ok(result);
        })
        .WithName("AssignRiskAlert")
        .WithSummary("Assign risk alert for investigation")
        .Produces<AssignRiskAlertResponse>();

        group.MapPost("/{id:guid}/escalate", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new EscalateRiskAlertCommand(id));
            return Results.Ok(result);
        })
        .WithName("EscalateRiskAlert")
        .WithSummary("Escalate a risk alert")
        .Produces<EscalateRiskAlertResponse>();

        group.MapPost("/{id:guid}/resolve", async (Guid id, ResolveRiskAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ResolveRiskAlertCommand(id, request.UserId, request.Resolution));
            return Results.Ok(result);
        })
        .WithName("ResolveRiskAlert")
        .WithSummary("Resolve a risk alert")
        .Produces<ResolveRiskAlertResponse>();

    }
}

public record AcknowledgeRiskAlertRequest(Guid UserId);
public record AssignRiskAlertRequest(Guid UserId);
public record ResolveRiskAlertRequest(Guid UserId, string Resolution);
