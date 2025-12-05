using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Assign.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Escalate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class AmlAlertEndpoints() : CarterModule("microfinance")
{

    private const string AssignAmlAlert = "AssignAmlAlert";
    private const string CreateAmlAlert = "CreateAmlAlert";
    private const string EscalateAmlAlert = "EscalateAmlAlert";
    private const string GetAmlAlert = "GetAmlAlert";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/aml-alerts").WithTags("AML Alerts");

        group.MapPost("/", async (CreateAmlAlertCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/aml-alerts/{result.Id}", result);
        })
        .WithName(CreateAmlAlert)
        .WithSummary("Create a new AML alert")
        .Produces<CreateAmlAlertResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetAmlAlertRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetAmlAlert)
        .WithSummary("Get AML alert by ID")
        .Produces<AmlAlertResponse>();

        group.MapPost("/{id:guid}/assign", async (Guid id, AssignAmlAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AssignAmlAlertCommand(id, request.AssignedToId));
            return Results.Ok(result);
        })
        .WithName(AssignAmlAlert)
        .WithSummary("Assign AML alert to investigator")
        .Produces<AssignAmlAlertResponse>();

        group.MapPost("/{id:guid}/escalate", async (Guid id, EscalateAmlAlertRequest request, ISender sender) =>
        {
            var result = await sender.Send(new EscalateAmlAlertCommand(id, request.Reason));
            return Results.Ok(result);
        })
        .WithName(EscalateAmlAlert)
        .WithSummary("Escalate AML alert")
        .Produces<EscalateAmlAlertResponse>();

    }
}

public record AssignAmlAlertRequest(Guid AssignedToId);
public record EscalateAmlAlertRequest(string Reason);
