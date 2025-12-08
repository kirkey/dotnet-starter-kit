using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Cancel.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Queue.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Report Generations.
/// </summary>
public class ReportGenerationEndpoints : CarterModule
{
    private const string GetReportGeneration = "GetReportGeneration";
    private const string QueueReportGeneration = "QueueReportGeneration";
    private const string CancelReportGeneration = "CancelReportGeneration";
    private const string SearchReportGenerations = "SearchReportGenerations";

    /// <summary>
    /// Maps all Report Generation endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/report-generations").WithTags("Report Generations");

        group.MapPost("/search", async (SearchReportGenerationsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchReportGenerations)
            .WithSummary("Searches report generations")
            .Produces<PagedList<ReportGenerationResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/", async (QueueReportGenerationCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/report-generations/{response.Id}", response);
            })
            .WithName(QueueReportGeneration)
            .WithSummary("Queues a new report generation")
            .Produces<QueueReportGenerationResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetReportGenerationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetReportGeneration)
            .WithSummary("Gets a report generation by ID")
            .Produces<ReportGenerationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/cancel", async (DefaultIdType id, CancelReportGenerationCommand command, ISender sender) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(CancelReportGeneration)
            .WithSummary("Cancels a report generation")
            .Produces<CancelReportGenerationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
