using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CreditBureauReports.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Credit Bureau Reports.
/// </summary>
public class CreditBureauReportEndpoints : CarterModule
{

    private const string CreateCreditBureauReport = "CreateCreditBureauReport";
    private const string GetCreditBureauReport = "GetCreditBureauReport";
    private const string SearchCreditBureauReports = "SearchCreditBureauReports";

    /// <summary>
    /// Maps all Credit Bureau Report endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/credit-bureau-reports").WithTags("Credit Bureau Reports");

        group.MapPost("/", async (CreateCreditBureauReportCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/credit-bureau-reports/{response.Id}", response);
            })
            .WithName(CreateCreditBureauReport)
            .WithSummary("Creates a new credit bureau report")
            .Produces<CreateCreditBureauReportResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetCreditBureauReportRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetCreditBureauReport)
            .WithSummary("Gets a credit bureau report by ID")
            .Produces<CreditBureauReportResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCreditBureauReportsCommand command, ISender sender) =>
            {
                var result = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName(SearchCreditBureauReports)
            .WithSummary("Search credit bureau reports")
            .Produces<PagedList<CreditBureauReportSummaryResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
