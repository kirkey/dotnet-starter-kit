using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Assign.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Clear.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Close.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Confirm.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Escalate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.FileSar.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class AmlAlertEndpoints : CarterModule
{

    private const string AssignAmlAlert = "AssignAmlAlert";
    private const string ClearAmlAlert = "ClearAmlAlert";
    private const string CloseAmlAlert = "CloseAmlAlert";
    private const string ConfirmAmlAlert = "ConfirmAmlAlert";
    private const string CreateAmlAlert = "CreateAmlAlert";
    private const string EscalateAmlAlert = "EscalateAmlAlert";
    private const string FileSarAmlAlert = "FileSarAmlAlert";
    private const string GetAmlAlert = "GetAmlAlert";
    private const string SearchAmlAlerts = "SearchAmlAlerts";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/aml-alerts").WithTags("AML Alerts");

        group.MapPost("/", async (CreateAmlAlertCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/microfinance/aml-alerts/{result.Id}", result);
        })
        .WithName(CreateAmlAlert)
        .WithSummary("Create a new AML alert")
        .Produces<CreateAmlAlertResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetAmlAlertRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(GetAmlAlert)
        .WithSummary("Get AML alert by ID")
        .Produces<AmlAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/assign", async (DefaultIdType id, AssignAmlAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AssignAmlAlertCommand(id, request.AssignedToId)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(AssignAmlAlert)
        .WithSummary("Assign AML alert to investigator")
        .Produces<AssignAmlAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/escalate", async (DefaultIdType id, EscalateAmlAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new EscalateAmlAlertCommand(id, request.Reason)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(EscalateAmlAlert)
        .WithSummary("Escalate AML alert")
        .Produces<EscalateAmlAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/clear", async (DefaultIdType id, ClearAmlAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ClearAmlAlertCommand(id, request.ResolvedById, request.Notes)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ClearAmlAlert)
        .WithSummary("Clear AML alert as non-suspicious")
        .Produces<ClearAmlAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/confirm", async (DefaultIdType id, ConfirmAmlAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ConfirmAmlAlertCommand(id, request.ResolvedById, request.Notes)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ConfirmAmlAlert)
        .WithSummary("Confirm AML alert as suspicious activity")
        .Produces<ConfirmAmlAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/file-sar", async (DefaultIdType id, FileSarAmlAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new FileSarAmlAlertCommand(id, request.SarReference, request.FiledDate)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(FileSarAmlAlert)
        .WithSummary("File Suspicious Activity Report (SAR) for AML alert")
        .Produces<FileSarAmlAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/close", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new CloseAmlAlertCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(CloseAmlAlert)
        .WithSummary("Close a resolved AML alert")
        .Produces<CloseAmlAlertResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchAmlAlertsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchAmlAlerts)
        .WithSummary("Search AML alerts with filters and pagination")
        .Produces<PagedList<AmlAlertResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record AssignAmlAlertRequest(DefaultIdType AssignedToId);
public record EscalateAmlAlertRequest(string Reason);
public record ClearAmlAlertRequest(DefaultIdType ResolvedById, string Notes);
public record ConfirmAmlAlertRequest(DefaultIdType ResolvedById, string Notes);
public record FileSarAmlAlertRequest(string SarReference, DateOnly FiledDate);
