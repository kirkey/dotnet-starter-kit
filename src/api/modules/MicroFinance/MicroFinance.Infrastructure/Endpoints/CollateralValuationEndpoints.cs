using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Submit.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollateralValuationEndpoints : CarterModule
{

    private const string ApproveValuation = "ApproveValuation";
    private const string CreateCollateralValuation = "CreateCollateralValuation";
    private const string GetCollateralValuation = "GetCollateralValuation";
    private const string RejectValuation = "RejectValuation";
    private const string SearchCollateralValuations = "SearchCollateralValuations";
    private const string SubmitValuation = "SubmitValuation";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collateral-valuations").WithTags("Collateral Valuations");

        group.MapPost("/", async (CreateCollateralValuationCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/microfinance/collateral-valuations/{result.Id}", result);
        })
        .WithName(CreateCollateralValuation)
        .WithSummary("Create a new collateral valuation")
        .Produces<CreateCollateralValuationResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollateralValuationRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(GetCollateralValuation)
        .WithSummary("Get collateral valuation by ID")
        .Produces<CollateralValuationResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/submit", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new SubmitValuationCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SubmitValuation)
        .WithSummary("Submit valuation for approval")
        .Produces<SubmitValuationResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Submit, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ApproveValuationCommand(id, request.ApprovedById)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ApproveValuation)
        .WithSummary("Approve a collateral valuation")
        .Produces<ApproveValuationResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RejectValuationCommand(id, request.Reason)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(RejectValuation)
        .WithSummary("Reject a collateral valuation")
        .Produces<RejectValuationResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCollateralValuationsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchCollateralValuations)
        .WithSummary("Search collateral valuations")
        .Produces<PagedList<CollateralValuationSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record ApproveRequest(DefaultIdType ApprovedById);
public record RejectRequest(string Reason);
