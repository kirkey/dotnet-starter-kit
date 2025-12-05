using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Review.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Settle.v1;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Submit.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class InsuranceClaimEndpoints() : CarterModule("microfinance")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/insurance-claims").WithTags("insurance-claims");

        // Submit Claim
        group.MapPost("/", async (SubmitInsuranceClaimCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/insurance-claims/{response.Id}", response);
            })
            .WithName("SubmitInsuranceClaim")
            .WithSummary("Submits a new insurance claim")
            .Produces<SubmitInsuranceClaimResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetInsuranceClaimRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetInsuranceClaim")
            .WithSummary("Gets an insurance claim by ID")
            .Produces<InsuranceClaimResponse>();

        // Claim Workflow
        group.MapPost("/{id:guid}/review", async (Guid id, ReviewInsuranceClaimCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("ReviewInsuranceClaim")
            .WithSummary("Starts review of an insurance claim")
            .Produces<ReviewInsuranceClaimResponse>();

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveInsuranceClaimCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("ApproveInsuranceClaim")
            .WithSummary("Approves an insurance claim")
            .Produces<ApproveInsuranceClaimResponse>();

        group.MapPost("/{id:guid}/reject", async (Guid id, RejectInsuranceClaimCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("RejectInsuranceClaim")
            .WithSummary("Rejects an insurance claim")
            .Produces<RejectInsuranceClaimResponse>();

        group.MapPost("/{id:guid}/settle", async (Guid id, SettleInsuranceClaimCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SettleInsuranceClaim")
            .WithSummary("Settles an approved insurance claim")
            .Produces<SettleInsuranceClaimResponse>();

    }
}
