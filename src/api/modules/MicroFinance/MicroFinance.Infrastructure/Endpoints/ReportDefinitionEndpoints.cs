using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.ConfigureSchedule.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Delete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.DisableSchedule.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Report Definitions.
/// </summary>
public class ReportDefinitionEndpoints : CarterModule
{
    private const string ActivateReportDefinition = "ActivateReportDefinition";
    private const string ConfigureScheduleForReportDefinition = "ConfigureScheduleForReportDefinition";
    private const string CreateReportDefinition = "CreateReportDefinition";
    private const string DeactivateReportDefinition = "DeactivateReportDefinition";
    private const string DeleteReportDefinition = "DeleteReportDefinition";
    private const string DisableScheduleForReportDefinition = "DisableScheduleForReportDefinition";
    private const string GetReportDefinition = "GetReportDefinition";
    private const string SearchReportDefinitions = "SearchReportDefinitions";
    private const string UpdateReportDefinition = "UpdateReportDefinition";

    /// <summary>
    /// Maps all Report Definition endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/report-definitions").WithTags("Report Definitions");

        group.MapPost("/", async (CreateReportDefinitionCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/report-definitions/{response.Id}", response);
            })
            .WithName(CreateReportDefinition)
            .WithSummary("Creates a new report definition")
            .Produces<CreateReportDefinitionResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetReportDefinitionRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetReportDefinition)
            .WithSummary("Gets a report definition by ID")
            .Produces<ReportDefinitionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/search", async ([FromBody] SearchReportDefinitionsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchReportDefinitions)
            .WithSummary("Searches report definitions with filters and pagination")
            .Produces<PagedList<ReportDefinitionResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id}/activate", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new ActivateReportDefinitionCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ActivateReportDefinition)
            .WithSummary("Activates a report definition")
            .Produces<ActivateReportDefinitionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Activate, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id}/deactivate", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new DeactivateReportDefinitionCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(DeactivateReportDefinition)
            .WithSummary("Deactivates a report definition")
            .Produces<DeactivateReportDefinitionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateReportDefinitionCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(UpdateReportDefinition)
            .WithSummary("Updates a report definition")
            .Produces<UpdateReportDefinitionResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                await sender.Send(new DeleteReportDefinitionCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(DeleteReportDefinition)
            .WithSummary("Deletes a report definition")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id}/configure-schedule", async (DefaultIdType id, ConfigureScheduleCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ConfigureScheduleForReportDefinition)
            .WithSummary("Configures schedule for a report definition")
            .Produces<ConfigureScheduleResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id}/disable-schedule", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new DisableScheduleCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(DisableScheduleForReportDefinition)
            .WithSummary("Disables schedule for a report definition")
            .Produces<DisableScheduleResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
