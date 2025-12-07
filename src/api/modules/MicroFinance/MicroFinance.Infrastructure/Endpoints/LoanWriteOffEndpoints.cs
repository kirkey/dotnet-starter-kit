using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Process.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.RecordRecovery.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class LoanWriteOffEndpoints() : CarterModule
{

    private const string ApproveWriteOff = "ApproveWriteOff";
    private const string CreateLoanWriteOff = "CreateLoanWriteOff";
    private const string GetLoanWriteOff = "GetLoanWriteOff";
    private const string ProcessWriteOff = "ProcessWriteOff";
    private const string RecordWriteOffRecovery = "RecordWriteOffRecovery";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/loan-write-offs").WithTags("Loan Write-Offs");

        group.MapPost("/", async (CreateLoanWriteOffCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/loan-write-offs/{result.Id}", result);
        })
        .WithName(CreateLoanWriteOff)
        .WithSummary("Create a new loan write-off")
        .Produces<CreateLoanWriteOffResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetLoanWriteOffRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetLoanWriteOff)
        .WithSummary("Get loan write-off by ID")
        .Produces<LoanWriteOffResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveWriteOffRequest request, ISender sender) =>
        {
            var command = new ApproveWriteOffCommand(id, request.UserId, request.ApproverName, request.WriteOffDate);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(ApproveWriteOff)
        .WithSummary("Approve loan write-off")
        .Produces<ApproveWriteOffResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/process", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new ProcessWriteOffCommand(id));
            return Results.Ok(result);
        })
        .WithName(ProcessWriteOff)
        .WithSummary("Process loan write-off")
        .Produces<ProcessWriteOffResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/record-recovery", async (Guid id, RecordRecoveryRequest request, ISender sender) =>
        {
            var command = new RecordRecoveryCommand(id, request.Amount, request.Notes);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(RecordWriteOffRecovery)
        .WithSummary("Record recovery on write-off")
        .Produces<RecordRecoveryResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public sealed record ApproveWriteOffRequest(Guid UserId, string ApproverName, DateOnly WriteOffDate);
public sealed record RecordRecoveryRequest(decimal Amount, string? Notes);
