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
public static class LoanEndpoints
{
    /// <summary>
    /// Maps all Loan endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapLoanEndpoints(this IEndpointRouteBuilder app)
    {
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

        loansGroup.MapPut("/{id:guid}", async (Guid id, UpdateLoanCommand command, ISender sender) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID mismatch");
                }
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateLoan")
            .WithSummary("Updates a pending loan application")
            .Produces<UpdateLoanResponse>();

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

        return app;
    }
}
