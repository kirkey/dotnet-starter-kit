using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Assign.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Review.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Submit.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Withdraw.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class LoanApplicationEndpoints() : CarterModule("microfinance")
{

    private const string ApproveLoanApplication = "ApproveLoanApplication";
    private const string AssignLoanApplication = "AssignLoanApplication";
    private const string CreateLoanApplication = "CreateLoanApplication";
    private const string GetLoanApplication = "GetLoanApplication";
    private const string RejectLoanApplication = "RejectLoanApplication";
    private const string ReviewLoanApplication = "ReviewLoanApplication";
    private const string SubmitLoanApplication = "SubmitLoanApplication";
    private const string WithdrawLoanApplication = "WithdrawLoanApplication";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/loan-applications").WithTags("loan-applications");

        // Create Application
        group.MapPost("/", async (CreateLoanApplicationCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/loan-applications/{response.Id}", response);
            })
            .WithName(CreateLoanApplication)
            .WithSummary("Creates a new loan application")
            .Produces<CreateLoanApplicationResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetLoanApplicationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetLoanApplication)
            .WithSummary("Gets a loan application by ID")
            .Produces<LoanApplicationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        // Application Workflow
        group.MapPost("/{id:guid}/submit", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new SubmitLoanApplicationCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SubmitLoanApplication)
            .WithSummary("Submits a loan application for review")
            .Produces<SubmitLoanApplicationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Submit, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/assign", async (Guid id, AssignLoanApplicationCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(AssignLoanApplication)
            .WithSummary("Assigns the application to a loan officer")
            .Produces<AssignLoanApplicationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/review", async (Guid id, ReviewLoanApplicationCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ReviewLoanApplication)
            .WithSummary("Completes review of the loan application")
            .Produces<ReviewLoanApplicationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveLoanApplicationCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ApproveLoanApplication)
            .WithSummary("Approves a loan application with terms")
            .Produces<ApproveLoanApplicationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reject", async (Guid id, RejectLoanApplicationCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RejectLoanApplication)
            .WithSummary("Rejects a loan application")
            .Produces<RejectLoanApplicationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/withdraw", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new WithdrawLoanApplicationCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(WithdrawLoanApplication)
            .WithSummary("Withdraws a loan application by the applicant")
            .Produces<WithdrawLoanApplicationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Withdraw, FshResources.MicroFinance))
            .MapToApiVersion(1);

    }
}
