using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Loan Schedules.
/// </summary>
public class LoanScheduleEndpoints() : CarterModule("microfinance")
{

    private const string GetLoanSchedule = "GetLoanSchedule";
    private const string GetLoanSchedulesByLoan = "GetLoanSchedulesByLoan";
    private const string SearchLoanSchedules = "SearchLoanSchedules";

    /// <summary>
    /// Maps all Loan Schedule endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var schedulesGroup = app.MapGroup("microfinance/loan-schedules").WithTags("loan-schedules");

        schedulesGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetLoanScheduleRequest(id));
            return Results.Ok(response);
        })
        .WithName(GetLoanSchedule)
        .WithSummary("Gets a loan schedule entry by ID")
        .Produces<LoanScheduleResponse>();

        schedulesGroup.MapPost("/search", async (SearchLoanSchedulesCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(SearchLoanSchedules)
        .WithSummary("Searches loan schedules with pagination")
        .Produces<PagedList<LoanScheduleResponse>>();

        schedulesGroup.MapGet("/by-loan/{loanId:guid}", async (Guid loanId, ISender mediator) =>
        {
            var request = new SearchLoanSchedulesCommand
            {
                LoanId = loanId,
                PageNumber = 1,
                PageSize = 100
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(GetLoanSchedulesByLoan)
        .WithSummary("Gets the complete repayment schedule for a loan")
        .Produces<PagedList<LoanScheduleResponse>>();
    }
}
