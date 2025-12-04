using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Queue.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Report Generations.
/// </summary>
public class ReportGenerationEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Report Generation endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/report-generations").WithTags("report-generations");

        group.MapPost("/", async (QueueReportGenerationCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/report-generations/{response.Id}", response);
            })
            .WithName("QueueReportGeneration")
            .WithSummary("Queues a new report generation")
            .Produces<QueueReportGenerationResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetReportGenerationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetReportGeneration")
            .WithSummary("Gets a report generation by ID")
            .Produces<ReportGenerationResponse>();
    }
}
