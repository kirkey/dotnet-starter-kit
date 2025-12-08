using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class LoanRestructureEndpoints : CarterModule
{

    private const string ApproveRestructure = "ApproveRestructure";
    private const string RejectRestructure = "RejectRestructure";
    private const string CreateLoanRestructure = "CreateLoanRestructure";
    private const string GetLoanRestructure = "GetLoanRestructure";
    private const string SearchLoanRestructures = "SearchLoanRestructures";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/loan-restructures").WithTags("Loan Restructures");

        group.MapPost("/", async (CreateLoanRestructureCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/loan-restructures/{result.Id}", result);
        })
        .WithName(CreateLoanRestructure)
        .WithSummary("Create a new loan restructure")
        .Produces<CreateLoanRestructureResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetLoanRestructureRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetLoanRestructure)
        .WithSummary("Get loan restructure by ID")
        .Produces<LoanRestructureResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveRestructureRequest request, ISender sender) =>
        {
            var command = new ApproveRestructureCommand(id, request.UserId, request.ApproverName, request.EffectiveDate);
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ApproveRestructure)
        .WithSummary("Approve loan restructure")
        .Produces<ApproveRestructureResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectRestructureRequest request, ISender sender) =>
        {
            var command = new RejectRestructureCommand(id, request.UserId, request.Reason);
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(RejectRestructure)
        .WithSummary("Reject loan restructure")
        .Produces<RejectRestructureResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchLoanRestructuresCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchLoanRestructures)
        .WithSummary("Search loan restructures")
        .Produces<PagedList<LoanRestructureSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public sealed record ApproveRestructureRequest(DefaultIdType UserId, string ApproverName, DateOnly EffectiveDate);
public sealed record RejectRestructureRequest(DefaultIdType UserId, string Reason);
