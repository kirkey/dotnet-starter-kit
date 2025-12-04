using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Delete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Update.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Close.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Disburse.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.WriteOff.v1;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Delete.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Members.Update.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Deposit.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Withdraw.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure;

/// <summary>
/// MicroFinance module endpoint routes.
/// Maps all microfinance endpoints with proper grouping and documentation.
/// </summary>
public class MicroFinanceEndpoints() : CarterModule("microfinance")
{
    /// <summary>
    /// Adds all microfinance routes to the application.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // ============================================
        // Members endpoints
        // ============================================
        var membersGroup = app.MapGroup("members").WithTags("members");

        membersGroup.MapPost("/", async (CreateMemberCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/members/{response.Id}", response);
            })
            .WithName("CreateMember")
            .WithSummary("Creates a new member")
            .Produces<CreateMemberResponse>(StatusCodes.Status201Created);

        membersGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetMemberRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetMember")
            .WithSummary("Gets a member by ID")
            .Produces<MemberResponse>();

        membersGroup.MapPut("/{id:guid}", async (Guid id, UpdateMemberCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateMember")
            .WithSummary("Updates a member")
            .Produces<UpdateMemberResponse>();

        membersGroup.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
            {
                await sender.Send(new DeleteMemberCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteMember")
            .WithSummary("Deletes a member")
            .Produces(StatusCodes.Status204NoContent);

        membersGroup.MapPost("/search", async ([FromBody] SearchMembersCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchMembers")
            .WithSummary("Searches members with filters and pagination")
            .Produces<PagedList<MemberResponse>>();

        // ============================================
        // Member Groups endpoints
        // ============================================
        var memberGroupsGroup = app.MapGroup("member-groups").WithTags("member-groups");

        memberGroupsGroup.MapPost("/", async (CreateMemberGroupCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/member-groups/{response.Id}", response);
            })
            .WithName("CreateMemberGroup")
            .WithSummary("Creates a new member group")
            .Produces<CreateMemberGroupResponse>(StatusCodes.Status201Created);

        // ============================================
        // Loan Products endpoints
        // ============================================
        var loanProductsGroup = app.MapGroup("loan-products").WithTags("loan-products");

        loanProductsGroup.MapPost("/", async (CreateLoanProductCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/loan-products/{response.Id}", response);
            })
            .WithName("CreateLoanProduct")
            .WithSummary("Creates a new loan product")
            .Produces<CreateLoanProductResponse>(StatusCodes.Status201Created);

        loanProductsGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetLoanProductRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetLoanProduct")
            .WithSummary("Gets a loan product by ID")
            .Produces<LoanProductResponse>();

        loanProductsGroup.MapPut("/{id:guid}", async (Guid id, UpdateLoanProductCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateLoanProduct")
            .WithSummary("Updates a loan product")
            .Produces<UpdateLoanProductResponse>();

        loanProductsGroup.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
            {
                await sender.Send(new DeleteLoanProductCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteLoanProduct")
            .WithSummary("Deletes a loan product")
            .Produces(StatusCodes.Status204NoContent);

        loanProductsGroup.MapPost("/search", async ([FromBody] SearchLoanProductsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchLoanProducts")
            .WithSummary("Searches loan products with filters and pagination")
            .Produces<PagedList<LoanProductResponse>>();

        // ============================================
        // Loans endpoints
        // ============================================
        var loansGroup = app.MapGroup("loans").WithTags("loans");

        loansGroup.MapPost("/", async (CreateLoanCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/loans/{response.Id}", response);
            })
            .WithName("CreateLoan")
            .WithSummary("Creates a new loan application")
            .Produces<CreateLoanResponse>(StatusCodes.Status201Created);

        loansGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetLoanRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetLoan")
            .WithSummary("Gets a loan by ID with full details")
            .Produces<LoanResponse>();

        loansGroup.MapPost("/{id:guid}/approve", async (Guid id, ApproveLoanCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("ApproveLoan")
            .WithSummary("Approves a pending loan")
            .Produces<ApproveLoanResponse>();

        loansGroup.MapPost("/{id:guid}/reject", async (Guid id, RejectLoanCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("RejectLoan")
            .WithSummary("Rejects a pending loan")
            .Produces<RejectLoanResponse>();

        loansGroup.MapPost("/{id:guid}/disburse", async (Guid id, DisburseLoanCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DisburseLoan")
            .WithSummary("Disburses an approved loan")
            .Produces<DisburseLoanResponse>();

        loansGroup.MapPost("/{id:guid}/close", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new CloseLoanCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CloseLoan")
            .WithSummary("Closes a fully paid loan")
            .Produces<CloseLoanResponse>();

        loansGroup.MapPost("/{id:guid}/write-off", async (Guid id, WriteOffLoanCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("WriteOffLoan")
            .WithSummary("Writes off a non-performing loan")
            .Produces<WriteOffLoanResponse>();

        loansGroup.MapPost("/search", async ([FromBody] SearchLoansCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchLoans")
            .WithSummary("Searches loans with filters and pagination")
            .Produces<PagedList<LoanSummaryResponse>>();

        // ============================================
        // Loan Repayments endpoints
        // ============================================
        var loanRepaymentsGroup = app.MapGroup("loan-repayments").WithTags("loan-repayments");

        loanRepaymentsGroup.MapPost("/", async (CreateLoanRepaymentCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/loan-repayments/{response.Id}", response);
            })
            .WithName("CreateLoanRepayment")
            .WithSummary("Records a loan repayment")
            .Produces<CreateLoanRepaymentResponse>(StatusCodes.Status201Created);

        // ============================================
        // Savings Products endpoints
        // ============================================
        var savingsProductsGroup = app.MapGroup("savings-products").WithTags("savings-products");

        savingsProductsGroup.MapPost("/", async (CreateSavingsProductCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/savings-products/{response.Id}", response);
            })
            .WithName("CreateSavingsProduct")
            .WithSummary("Creates a new savings product")
            .Produces<CreateSavingsProductResponse>(StatusCodes.Status201Created);

        savingsProductsGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetSavingsProductRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetSavingsProduct")
            .WithSummary("Gets a savings product by ID")
            .Produces<SavingsProductResponse>();

        // ============================================
        // Savings Accounts endpoints
        // ============================================
        var savingsAccountsGroup = app.MapGroup("savings-accounts").WithTags("savings-accounts");

        savingsAccountsGroup.MapPost("/", async (CreateSavingsAccountCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/savings-accounts/{response.Id}", response);
            })
            .WithName("CreateSavingsAccount")
            .WithSummary("Creates a new savings account")
            .Produces<CreateSavingsAccountResponse>(StatusCodes.Status201Created);

        savingsAccountsGroup.MapPost("/{id:guid}/deposit", async (Guid id, DepositCommand command, ISender sender) =>
            {
                if (id != command.AccountId)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DepositToSavingsAccount")
            .WithSummary("Deposits money to a savings account")
            .Produces<DepositResponse>();

        savingsAccountsGroup.MapPost("/{id:guid}/withdraw", async (Guid id, WithdrawCommand command, ISender sender) =>
            {
                if (id != command.AccountId)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("WithdrawFromSavingsAccount")
            .WithSummary("Withdraws money from a savings account")
            .Produces<WithdrawResponse>();

        // ============================================
        // Fixed Deposits endpoints
        // ============================================
        var fixedDepositsGroup = app.MapGroup("fixed-deposits").WithTags("fixed-deposits");
        fixedDepositsGroup.MapGet("/", () => Results.Ok("Fixed Deposits endpoint - Coming soon"))
            .WithName("GetFixedDeposits")
            .WithSummary("Gets all fixed deposits");

        // ============================================
        // Share Products endpoints
        // ============================================
        var shareProductsGroup = app.MapGroup("share-products").WithTags("share-products");
        shareProductsGroup.MapGet("/", () => Results.Ok("Share Products endpoint - Coming soon"))
            .WithName("GetShareProducts")
            .WithSummary("Gets all share products");

        // ============================================
        // Share Accounts endpoints
        // ============================================
        var shareAccountsGroup = app.MapGroup("share-accounts").WithTags("share-accounts");
        shareAccountsGroup.MapGet("/", () => Results.Ok("Share Accounts endpoint - Coming soon"))
            .WithName("GetShareAccounts")
            .WithSummary("Gets all share accounts");

        // ============================================
        // Fee Definitions endpoints
        // ============================================
        var feeDefinitionsGroup = app.MapGroup("fee-definitions").WithTags("fee-definitions");
        feeDefinitionsGroup.MapGet("/", () => Results.Ok("Fee Definitions endpoint - Coming soon"))
            .WithName("GetFeeDefinitions")
            .WithSummary("Gets all fee definitions");
    }
}

