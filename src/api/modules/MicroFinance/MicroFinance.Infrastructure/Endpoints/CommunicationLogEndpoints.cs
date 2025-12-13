using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.MarkDelivered.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CommunicationLogEndpoints : CarterModule
{

    private const string CreateCommunicationLog = "CreateCommunicationLog";
    private const string GetCommunicationLog = "GetCommunicationLog";
    private const string MarkCommunicationDelivered = "MarkCommunicationDelivered";
    private const string SearchCommunicationLogs = "SearchCommunicationLogs";

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

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCommunicationLogRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCommunicationLog)
        .WithSummary("Get communication log by ID")
        .Produces<CommunicationLogResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id}/delivered", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new MarkCommunicationDeliveredCommand(id));
            return Results.Ok(result);
        })
        .WithName(MarkCommunicationDelivered)
        .WithSummary("Mark communication as delivered")
        .Produces<MarkCommunicationDeliveredResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCommunicationLogsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchCommunicationLogs)
        .WithSummary("Search communication logs")
        .Produces<PagedList<CommunicationLogSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
