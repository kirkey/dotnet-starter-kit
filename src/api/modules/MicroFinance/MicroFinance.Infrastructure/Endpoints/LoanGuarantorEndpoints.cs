using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Release.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.UpdateAmount.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Loan Guarantors.
/// </summary>
public class LoanGuarantorEndpoints() : CarterModule("microfinance")
{

    private const string ApproveLoanGuarantor = "ApproveLoanGuarantor";
    private const string CreateLoanGuarantor = "CreateLoanGuarantor";
    private const string GetLoanGuarantor = "GetLoanGuarantor";
    private const string GetLoanGuarantorsByLoan = "GetLoanGuarantorsByLoan";
    private const string GetLoanGuarantorsByMember = "GetLoanGuarantorsByMember";
    private const string RejectLoanGuarantor = "RejectLoanGuarantor";
    private const string ReleaseLoanGuarantor = "ReleaseLoanGuarantor";
    private const string SearchLoanGuarantors = "SearchLoanGuarantors";
    private const string UpdateGuarantorAmount = "UpdateGuarantorAmount";

    /// <summary>
    /// Maps all Loan Guarantor endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var guarantorsGroup = app.MapGroup("microfinance/loan-guarantors").WithTags("loan-guarantors");

        guarantorsGroup.MapPost("/", async (CreateLoanGuarantorCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command);
            return Results.Created($"/microfinance/loan-guarantors/{response.Id}", response);
        })
        .WithName(CreateLoanGuarantor)
        .WithSummary("Creates a new loan guarantor")
        .Produces<CreateLoanGuarantorResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        guarantorsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetLoanGuarantorRequest(id));
            return Results.Ok(response);
        })
        .WithName(GetLoanGuarantor)
        .WithSummary("Gets a loan guarantor by ID")
        .Produces<LoanGuarantorResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        guarantorsGroup.MapPost("/search", async (SearchLoanGuarantorsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(SearchLoanGuarantors)
        .WithSummary("Searches loan guarantors with pagination")
        .Produces<PagedList<LoanGuarantorResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
        .MapToApiVersion(1);

        guarantorsGroup.MapGet("/by-loan/{loanId:guid}", async (Guid loanId, ISender mediator) =>
        {
            var response = await mediator.Send(new SearchLoanGuarantorsCommand
            {
                LoanId = loanId,
                PageNumber = 1,
                PageSize = 50
            });
            return Results.Ok(response);
        })
        .WithName(GetLoanGuarantorsByLoan)
        .WithSummary("Gets all guarantors for a loan")
        .Produces<PagedList<LoanGuarantorResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        guarantorsGroup.MapGet("/by-member/{memberId:guid}", async (Guid memberId, ISender mediator) =>
        {
            var response = await mediator.Send(new SearchLoanGuarantorsCommand
            {
                GuarantorMemberId = memberId,
                PageNumber = 1,
                PageSize = 50
            });
            return Results.Ok(response);
        })
        .WithName(GetLoanGuarantorsByMember)
        .WithSummary("Gets all loans where a member is a guarantor")
        .Produces<PagedList<LoanGuarantorResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        guarantorsGroup.MapPost("/{id:guid}/approve", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new ApproveGuarantorCommand(id));
            return Results.Ok(response);
        })
        .WithName(ApproveLoanGuarantor)
        .WithSummary("Approves a loan guarantor")
        .Produces<ApproveGuarantorResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        guarantorsGroup.MapPost("/{id:guid}/reject", async (Guid id, RejectGuarantorCommand command, ISender mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(RejectLoanGuarantor)
        .WithSummary("Rejects a loan guarantor")
        .Produces<RejectGuarantorResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.MicroFinance))
        .MapToApiVersion(1);

        guarantorsGroup.MapPost("/{id:guid}/release", async (Guid id, ReleaseGuarantorCommand command, ISender mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(ReleaseLoanGuarantor)
        .WithSummary("Releases a loan guarantor")
        .Produces<ReleaseGuarantorResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        guarantorsGroup.MapPut("/{id:guid}/amount", async (Guid id, UpdateGuaranteedAmountCommand command, ISender mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(UpdateGuarantorAmount)
        .WithSummary("Updates the guaranteed amount for a loan guarantor")
        .Produces<UpdateGuaranteedAmountResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);
    }
}
