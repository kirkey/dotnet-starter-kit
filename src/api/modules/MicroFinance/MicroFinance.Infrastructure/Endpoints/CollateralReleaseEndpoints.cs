using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Release.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollateralReleaseEndpoints() : CarterModule("microfinance")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collateral-releases").WithTags("Collateral Releases");

        group.MapPost("/", async (CreateCollateralReleaseCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collateral-releases/{result.Id}", result);
        })
        .WithName("CreateCollateralRelease")
        .WithSummary("Create a new collateral release request")
        .Produces<CreateCollateralReleaseResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollateralReleaseRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetCollateralRelease")
        .WithSummary("Get collateral release by ID")
        .Produces<CollateralReleaseResponse>();

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveReleaseRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ApproveReleaseCommand(id, request.ApprovedById));
            return Results.Ok(result);
        })
        .WithName("ApproveRelease")
        .WithSummary("Approve a collateral release")
        .Produces<ApproveReleaseResponse>();

        group.MapPost("/{id:guid}/release", async (Guid id, ReleaseRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ReleaseCollateralCommand(
                id,
                request.ReleasedById,
                request.RecipientName,
                request.RecipientIdNumber,
                request.RecipientSignaturePath));
            return Results.Ok(result);
        })
        .WithName("ReleaseCollateral")
        .WithSummary("Complete collateral release")
        .Produces<ReleaseCollateralResponse>();

    }
}

public record ApproveReleaseRequest(Guid ApprovedById);
public record ReleaseRequest(Guid ReleasedById, string RecipientName, string? RecipientIdNumber, string? RecipientSignaturePath);
