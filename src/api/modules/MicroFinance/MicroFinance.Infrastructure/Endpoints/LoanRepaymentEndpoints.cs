using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRepayments.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Loan Repayments.
/// </summary>
public class LoanRepaymentEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Loan Repayment endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var loanRepaymentsGroup = app.MapGroup("microfinance/loan-repayments").WithTags("loan-repayments");

        loanRepaymentsGroup.MapPost("/", async (CreateLoanRepaymentCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/loan-repayments/{response.Id}", response);
            })
            .WithName("CreateLoanRepayment")
            .WithSummary("Records a loan repayment")
            .Produces<CreateLoanRepaymentResponse>(StatusCodes.Status201Created);

        loanRepaymentsGroup.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetLoanRepaymentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetLoanRepayment")
            .WithSummary("Gets a loan repayment by ID")
            .Produces<LoanRepaymentResponse>();

        loanRepaymentsGroup.MapPost("/search", async (SearchLoanRepaymentsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchLoanRepayments")
            .WithSummary("Searches loan repayments with filters and pagination")
            .Produces<PagedList<LoanRepaymentResponse>>();

        loanRepaymentsGroup.MapGet("/by-loan/{loanId:guid}", async (Guid loanId, ISender sender) =>
            {
                var command = new SearchLoanRepaymentsCommand
                {
                    LoanId = loanId,
                    PageNumber = 1,
                    PageSize = 100
                };
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetLoanRepaymentsByLoan")
            .WithSummary("Gets all repayments for a specific loan")
            .Produces<PagedList<LoanRepaymentResponse>>();
    }
}
