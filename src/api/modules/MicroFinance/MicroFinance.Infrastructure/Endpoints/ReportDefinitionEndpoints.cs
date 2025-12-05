using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Report Definitions.
/// </summary>
public class ReportDefinitionEndpoints() : CarterModule("microfinance")
{

    private const string ActivateReportDefinition = "ActivateReportDefinition";
    private const string CreateReportDefinition = "CreateReportDefinition";
    private const string GetReportDefinition = "GetReportDefinition";

    /// <summary>
    /// Maps all Report Definition endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/report-definitions").WithTags("report-definitions");

        group.MapPost("/", async (CreateReportDefinitionCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/report-definitions/{response.Id}", response);
            })
            .WithName(CreateReportDefinition)
            .WithSummary("Creates a new report definition")
            .Produces<CreateReportDefinitionResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetReportDefinitionRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetReportDefinition)
            .WithSummary("Gets a report definition by ID")
            .Produces<ReportDefinitionResponse>();

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new ActivateReportDefinitionCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ActivateReportDefinition)
            .WithSummary("Activates a report definition")
            .Produces<ActivateReportDefinitionResponse>();
    }
}
