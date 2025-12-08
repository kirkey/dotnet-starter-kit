using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Release.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollateralReleaseEndpoints : CarterModule
{

    private const string ApproveRelease = "ApproveRelease";
    private const string CreateCollateralRelease = "CreateCollateralRelease";
    private const string GetCollateralRelease = "GetCollateralRelease";
    private const string CompleteCollateralRelease = "CompleteCollateralRelease";

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

    }
}

public record ApproveReleaseRequest(DefaultIdType ApprovedById);
public record ReleaseRequest(DefaultIdType ReleasedById, string RecipientName, string? RecipientIdNumber, string? RecipientSignaturePath);
