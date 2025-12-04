using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Search.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Loan Guarantors.
/// </summary>
public static class LoanGuarantorEndpoints
{
    /// <summary>
    /// Maps all Loan Guarantor endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapLoanGuarantorEndpoints(this IEndpointRouteBuilder app)
    {
        var guarantorsGroup = app.MapGroup("loan-guarantors").WithTags("loan-guarantors");

        guarantorsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetLoanGuarantorRequest(id));
            return Results.Ok(response);
        })
        .WithName("GetLoanGuarantor")
        .WithSummary("Gets a loan guarantor by ID")
        .Produces<LoanGuarantorResponse>();

        guarantorsGroup.MapPost("/search", async (SearchLoanGuarantorsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("SearchLoanGuarantors")
        .WithSummary("Searches loan guarantors with pagination")
        .Produces<PagedList<LoanGuarantorResponse>>();

        guarantorsGroup.MapGet("/by-loan/{loanId:guid}", async (Guid loanId, ISender mediator) =>
        {
            var request = new SearchLoanGuarantorsCommand
            {
                LoanId = loanId,
                PageNumber = 1,
                PageSize = 50
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("GetLoanGuarantorsByLoan")
        .WithSummary("Gets all guarantors for a loan")
        .Produces<PagedList<LoanGuarantorResponse>>();

        guarantorsGroup.MapGet("/by-member/{memberId:guid}", async (Guid memberId, ISender mediator) =>
        {
            var request = new SearchLoanGuarantorsCommand
            {
                GuarantorMemberId = memberId,
                PageNumber = 1,
                PageSize = 50
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("GetLoanGuarantorsByMember")
        .WithSummary("Gets all loans where a member is a guarantor")
        .Produces<PagedList<LoanGuarantorResponse>>();

        return app;
    }
}
