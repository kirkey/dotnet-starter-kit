using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Disburse.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.VerifyMilestone.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class LoanDisbursementTrancheEndpoints : CarterModule
{

    private const string ApproveTranche = "ApproveTranche";
    private const string CreateLoanDisbursementTranche = "CreateLoanDisbursementTranche";
    private const string DisburseTranche = "DisburseTranche";
    private const string GetLoanDisbursementTranche = "GetLoanDisbursementTranche";
    private const string VerifyTrancheMilestone = "VerifyTrancheMilestone";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/loan-disbursement-tranches").WithTags("Loan Disbursement Tranches");

        group.MapPost("/", async (CreateLoanDisbursementTrancheCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/loan-disbursement-tranches/{result.Id}", result);
        })
        .WithName(CreateLoanDisbursementTranche)
        .WithSummary("Create a new loan disbursement tranche")
        .Produces<CreateLoanDisbursementTrancheResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetLoanDisbursementTrancheRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetLoanDisbursementTranche)
        .WithSummary("Get loan disbursement tranche by ID")
        .Produces<LoanDisbursementTrancheResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/verify-milestone", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new VerifyMilestoneCommand(id));
            return Results.Ok(result);
        })
        .WithName(VerifyTrancheMilestone)
        .WithSummary("Verify tranche milestone")
        .Produces<VerifyMilestoneResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveTrancheRequest request, ISender sender) =>
        {
            var command = new ApproveTrancheCommand(id, request.UserId);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(ApproveTranche)
        .WithSummary("Approve disbursement tranche")
        .Produces<ApproveTrancheResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/disburse", async (DefaultIdType id, DisburseTrancheRequest request, ISender sender) =>
        {
            var command = new DisburseTrancheCommand(id, request.UserId, request.ReferenceNumber, request.DisbursedDate);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(DisburseTranche)
        .WithSummary("Disburse tranche")
        .Produces<DisburseTrancheResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Disburse, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public sealed record ApproveTrancheRequest(DefaultIdType UserId);
public sealed record DisburseTrancheRequest(DefaultIdType UserId, string ReferenceNumber, DateOnly? DisbursedDate);
