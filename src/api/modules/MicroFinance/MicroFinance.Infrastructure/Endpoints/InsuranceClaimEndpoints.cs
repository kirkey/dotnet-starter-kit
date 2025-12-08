using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Review.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Settle.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Submit.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class InsuranceClaimEndpoints : CarterModule
{

    private const string ApproveInsuranceClaim = "ApproveInsuranceClaim";
    private const string GetInsuranceClaim = "GetInsuranceClaim";
    private const string RejectInsuranceClaim = "RejectInsuranceClaim";
    private const string ReviewInsuranceClaim = "ReviewInsuranceClaim";
    private const string SettleInsuranceClaim = "SettleInsuranceClaim";
    private const string SubmitInsuranceClaim = "SubmitInsuranceClaim";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/insurance-claims").WithTags("Insurance Claims");

        // Submit Claim
        group.MapPost("/", async (SubmitInsuranceClaimCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/insurance-claims/{response.Id}", response);
            })
            .WithName(SubmitInsuranceClaim)
            .WithSummary("Submits a new insurance claim")
            .Produces<SubmitInsuranceClaimResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Submit, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetInsuranceClaimRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetInsuranceClaim)
            .WithSummary("Gets an insurance claim by ID")
            .Produces<InsuranceClaimResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Claim Workflow
        group.MapPost("/{id:guid}/review", async (DefaultIdType id, ReviewInsuranceClaimCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ReviewInsuranceClaim)
            .WithSummary("Starts review of an insurance claim")
            .Produces<ReviewInsuranceClaimResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveInsuranceClaimCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ApproveInsuranceClaim)
            .WithSummary("Approves an insurance claim")
            .Produces<ApproveInsuranceClaimResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectInsuranceClaimCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RejectInsuranceClaim)
            .WithSummary("Rejects an insurance claim")
            .Produces<RejectInsuranceClaimResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/settle", async (DefaultIdType id, SettleInsuranceClaimCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SettleInsuranceClaim)
            .WithSummary("Settles an approved insurance claim")
            .Produces<SettleInsuranceClaimResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

    }
}
