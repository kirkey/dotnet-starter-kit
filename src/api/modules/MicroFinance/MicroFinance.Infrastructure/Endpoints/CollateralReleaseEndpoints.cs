using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Cancel.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Release.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollateralReleaseEndpoints : CarterModule
{

    private const string ApproveRelease = "ApproveRelease";
    private const string RejectRelease = "RejectRelease";
    private const string CancelRelease = "CancelRelease";
    private const string CreateCollateralRelease = "CreateCollateralRelease";
    private const string GetCollateralRelease = "GetCollateralRelease";
    private const string CompleteCollateralRelease = "CompleteCollateralRelease";
    private const string SearchCollateralReleases = "SearchCollateralReleases";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collateral-releases").WithTags("Collateral Releases");

        group.MapPost("/", async (CreateCollateralReleaseCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collateral-releases/{result.Id}", result);
        })
        .WithName(CreateCollateralRelease)
        .WithSummary("Create a new collateral release request")
        .Produces<CreateCollateralReleaseResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollateralReleaseRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCollateralRelease)
        .WithSummary("Get collateral release by ID")
        .Produces<CollateralReleaseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveReleaseRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ApproveReleaseCommand(id, request.ApprovedById));
            return Results.Ok(result);
        })
        .WithName(ApproveRelease)
        .WithSummary("Approve a collateral release")
        .Produces<ApproveReleaseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/release", async (DefaultIdType id, ReleaseRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CompleteCollateralReleaseCommand(
                id,
                request.ReleasedById,
                request.RecipientName,
                request.RecipientIdNumber,
                request.RecipientSignaturePath));
            return Results.Ok(result);
        })
        .WithName(CompleteCollateralRelease)
        .WithSummary("Complete collateral release")
        .Produces<CompleteCollateralReleaseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchCollateralReleasesCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchCollateralReleases)
        .WithSummary("Search collateral releases")
        .Produces<PagedList<CollateralReleaseSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectReleaseRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RejectCollateralReleaseCommand(id, request.Reason, request.RejectedById)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(RejectRelease)
        .WithSummary("Reject a collateral release request")
        .Produces<RejectCollateralReleaseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/cancel", async (DefaultIdType id, CancelReleaseRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CancelCollateralReleaseCommand(id, request.Reason)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(CancelRelease)
        .WithSummary("Cancel a collateral release request")
        .Produces<CancelCollateralReleaseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record ApproveReleaseRequest(DefaultIdType ApprovedById);
public record RejectReleaseRequest(string Reason, DefaultIdType RejectedById);
public record CancelReleaseRequest(string Reason);
public record ReleaseRequest(DefaultIdType ReleasedById, string RecipientName, string? RecipientIdNumber, string? RecipientSignaturePath);
