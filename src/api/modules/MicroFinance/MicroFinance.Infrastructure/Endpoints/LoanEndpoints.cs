using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Close.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Disburse.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Update.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.WriteOff.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Loans.
/// </summary>
public class LoanEndpoints : CarterModule
{

    private const string ApproveLoan = "ApproveLoan";
    private const string CloseLoan = "CloseLoan";
    private const string CreateLoan = "CreateLoan";
    private const string DisburseLoan = "DisburseLoan";
    private const string GetLoan = "GetLoan";
    private const string RejectLoan = "RejectLoan";
    private const string SearchLoans = "SearchLoans";
    private const string UpdateLoan = "UpdateLoan";
    private const string WriteOffLoan = "WriteOffLoan";

    /// <summary>
    /// Maps all Loan endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var loansGroup = app.MapGroup("microfinance/loans").WithTags("Loans");

        loansGroup.MapPost("/", async (CreateLoanCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/loans/{response.Id}", response);
            })
            .WithName(CreateLoan)
            .WithSummary("Creates a new loan application")
            .Produces<CreateLoanResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loansGroup.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetLoanRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetLoan)
            .WithSummary("Gets a loan by ID with full details")
            .Produces<LoanResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loansGroup.MapPut("/{id:guid}", async (DefaultIdType id, UpdateLoanCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(UpdateLoan)
            .WithSummary("Updates a pending loan application")
            .Produces<UpdateLoanResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loansGroup.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveLoanCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ApproveLoan)
            .WithSummary("Approves a pending loan")
            .Produces<ApproveLoanResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loansGroup.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectLoanCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(RejectLoan)
            .WithSummary("Rejects a pending loan")
            .Produces<RejectLoanResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loansGroup.MapPost("/{id:guid}/disburse", async (DefaultIdType id, DisburseLoanCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(DisburseLoan)
            .WithSummary("Disburses an approved loan")
            .Produces<DisburseLoanResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Disburse, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loansGroup.MapPost("/{id:guid}/close", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new CloseLoanCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(CloseLoan)
            .WithSummary("Closes a fully paid loan")
            .Produces<CloseLoanResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Close, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loansGroup.MapPost("/{id:guid}/write-off", async (DefaultIdType id, WriteOffLoanCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(WriteOffLoan)
            .WithSummary("Writes off a non-performing loan")
            .Produces<WriteOffLoanResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.WriteOff, FshResources.MicroFinance))
            .MapToApiVersion(1);

        loansGroup.MapPost("/search", async ([FromBody] SearchLoansCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchLoans)
            .WithSummary("Searches loans with filters and pagination")
            .Produces<PagedList<LoanSummaryResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
