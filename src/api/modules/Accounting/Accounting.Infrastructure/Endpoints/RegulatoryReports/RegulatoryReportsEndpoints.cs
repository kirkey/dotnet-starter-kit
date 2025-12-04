using Accounting.Application.RegulatoryReports.Create.v1;
using Accounting.Application.RegulatoryReports.Get.v1;
using Accounting.Application.RegulatoryReports.Responses;
using Accounting.Application.RegulatoryReports.Search.v1;
using Accounting.Application.RegulatoryReports.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports;

/// <summary>
/// Endpoint configuration for Regulatory Reports module.
/// Provides comprehensive REST API endpoints for managing regulatory reports.
/// </summary>
public class RegulatoryReportsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Regulatory Reports endpoints to the route builder.
    /// Includes Create, Read, Update, and Search operations.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/regulatory-reports").WithTags("regulatory-reports");

        // Create endpoint
        group.MapPost("/", async (RegulatoryReportCreateRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreateRegulatoryReport")
            .WithSummary("Create a new regulatory report")
            .WithDescription("Creates a new regulatory report")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetRegulatoryReportRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetRegulatoryReport")
            .WithSummary("Get regulatory report by ID")
            .WithDescription("Retrieves a regulatory report by its unique identifier")
            .Produces<RegulatoryReportResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateRegulatoryReportRequest request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID in URL does not match ID in request body");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateRegulatoryReport")
            .WithSummary("Update regulatory report")
            .WithDescription("Updates an existing regulatory report")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search endpoint
        group.MapPost("/search", async (SearchRegulatoryReportsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchRegulatoryReports")
            .WithSummary("Search regulatory reports")
            .WithDescription("Searches regulatory reports with filtering support")
            .Produces<List<RegulatoryReportResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
