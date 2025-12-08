using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Acknowledge.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Assign.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Escalate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Resolve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class RiskAlertEndpoints : CarterModule
{

    private const string AcknowledgeRiskAlert = "AcknowledgeRiskAlert";
    private const string AssignRiskAlert = "AssignRiskAlert";
    private const string CreateRiskAlert = "CreateRiskAlert";
    private const string EscalateRiskAlert = "EscalateRiskAlert";
    private const string GetRiskAlert = "GetRiskAlert";
    private const string ResolveRiskAlert = "ResolveRiskAlert";
    private const string SearchRiskAlerts = "SearchRiskAlerts";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/risk-alerts").WithTags("Risk Alerts");

        group.MapPost("/", async (CreateRiskAlertCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/risk-alerts/{result.Id}", result);
        })
        .WithName(CreateRiskAlert)
        .WithSummary("Create a new risk alert")
        .Produces<CreateRiskAlertResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetRiskAlertRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetRiskAlert)
        .WithSummary("Get risk alert by ID")
        .Produces<RiskAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/acknowledge", async (DefaultIdType id, AcknowledgeRiskAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AcknowledgeRiskAlertCommand(id, request.UserId));
            return Results.Ok(result);
        })
        .WithName(AcknowledgeRiskAlert)
        .WithSummary("Acknowledge a risk alert")
        .Produces<AcknowledgeRiskAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/assign", async (DefaultIdType id, AssignRiskAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AssignRiskAlertCommand(id, request.UserId));
            return Results.Ok(result);
        })
        .WithName(AssignRiskAlert)
        .WithSummary("Assign risk alert for investigation")
        .Produces<AssignRiskAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/escalate", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new EscalateRiskAlertCommand(id));
            return Results.Ok(result);
        })
        .WithName(EscalateRiskAlert)
        .WithSummary("Escalate a risk alert")
        .Produces<EscalateRiskAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/resolve", async (DefaultIdType id, ResolveRiskAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ResolveRiskAlertCommand(id, request.UserId, request.Resolution));
            return Results.Ok(result);
        })
        .WithName(ResolveRiskAlert)
        .WithSummary("Resolve a risk alert")
        .Produces<ResolveRiskAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchRiskAlertsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchRiskAlerts)
        .WithSummary("Search risk alerts")
        .Produces<PagedList<RiskAlertSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record AcknowledgeRiskAlertRequest(DefaultIdType UserId);
public record AssignRiskAlertRequest(DefaultIdType UserId);
public record ResolveRiskAlertRequest(DefaultIdType UserId, string Resolution);
