using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.MarkDelivered.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CommunicationLogEndpoints() : CarterModule
{

    private const string CreateCommunicationLog = "CreateCommunicationLog";
    private const string GetCommunicationLog = "GetCommunicationLog";
    private const string MarkCommunicationDelivered = "MarkCommunicationDelivered";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/communication-logs").WithTags("Communication Logs");

        group.MapPost("/", async (CreateCommunicationLogCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/communication-logs/{result.Id}", result);
        })
        .WithName(CreateCommunicationLog)
        .WithSummary("Create a new communication log entry")
        .Produces<CreateCommunicationLogResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCommunicationLogRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCommunicationLog)
        .WithSummary("Get communication log by ID")
        .Produces<CommunicationLogResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/delivered", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new MarkCommunicationDeliveredCommand(id));
            return Results.Ok(result);
        })
        .WithName(MarkCommunicationDelivered)
        .WithSummary("Mark communication as delivered")
        .Produces<MarkCommunicationDeliveredResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
