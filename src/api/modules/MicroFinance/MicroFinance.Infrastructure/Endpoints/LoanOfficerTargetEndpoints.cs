using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.RecordProgress.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class LoanOfficerTargetEndpoints() : CarterModule("microfinance")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/loan-officer-targets").WithTags("Loan Officer Targets");

        group.MapPost("/", async (CreateLoanOfficerTargetCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/loan-officer-targets/{result.Id}", result);
        })
        .WithName("CreateLoanOfficerTarget")
        .WithSummary("Create a new loan officer target")
        .Produces<CreateLoanOfficerTargetResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetLoanOfficerTargetRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetLoanOfficerTarget")
        .WithSummary("Get loan officer target by ID")
        .Produces<LoanOfficerTargetResponse>();

        group.MapPost("/{id:guid}/progress", async (Guid id, RecordLoanOfficerProgressRequest request, ISender sender) =>
        {
            var command = new RecordLoanOfficerProgressCommand(id, request.AchievedValue);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("RecordLoanOfficerProgress")
        .WithSummary("Record progress towards loan officer target")
        .Produces<RecordLoanOfficerProgressResponse>();

    }
}

public sealed record RecordLoanOfficerProgressRequest(decimal AchievedValue);
