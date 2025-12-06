using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.RecordProgress.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class LoanOfficerTargetEndpoints() : CarterModule("microfinance")
{

    private const string CreateLoanOfficerTarget = "CreateLoanOfficerTarget";
    private const string GetLoanOfficerTarget = "GetLoanOfficerTarget";
    private const string RecordLoanOfficerProgress = "RecordLoanOfficerProgress";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/loan-officer-targets").WithTags("Loan Officer Targets");

        group.MapPost("/", async (CreateLoanOfficerTargetCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/loan-officer-targets/{result.Id}", result);
        })
        .WithName(CreateLoanOfficerTarget)
        .WithSummary("Create a new loan officer target")
        .Produces<CreateLoanOfficerTargetResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetLoanOfficerTargetRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetLoanOfficerTarget)
        .WithSummary("Get loan officer target by ID")
        .Produces<LoanOfficerTargetResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/progress", async (Guid id, RecordLoanOfficerProgressRequest request, ISender sender) =>
        {
            var command = new RecordLoanOfficerProgressCommand(id, request.AchievedValue);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(RecordLoanOfficerProgress)
        .WithSummary("Record progress towards loan officer target")
        .Produces<RecordLoanOfficerProgressResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public sealed record RecordLoanOfficerProgressRequest(decimal AchievedValue);
