using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.RecordProgress.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class BranchTargetEndpoints() : CarterModule("microfinance")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/branch-targets").WithTags("Branch Targets");

        group.MapPost("/", async (CreateBranchTargetCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/branch-targets/{result.Id}", result);
        })
        .WithName("CreateBranchTarget")
        .WithSummary("Create a new branch target")
        .Produces<CreateBranchTargetResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetBranchTargetRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetBranchTarget")
        .WithSummary("Get branch target by ID")
        .Produces<BranchTargetResponse>();

        group.MapPost("/{id:guid}/progress", async (Guid id, RecordBranchProgressRequest request, ISender sender) =>
        {
            var command = new RecordBranchProgressCommand(id, request.AchievedValue);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("RecordBranchProgress")
        .WithSummary("Record progress towards branch target")
        .Produces<RecordBranchProgressResponse>();

    }
}

public sealed record RecordBranchProgressRequest(decimal AchievedValue);
