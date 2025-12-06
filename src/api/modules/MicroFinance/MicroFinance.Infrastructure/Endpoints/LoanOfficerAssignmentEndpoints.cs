using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.AssignToGroup.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.AssignToMember.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.End.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Transfer.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class LoanOfficerAssignmentEndpoints() : CarterModule("microfinance")
{

    private const string AssignToGroup = "AssignToGroup";
    private const string AssignToMember = "AssignToMember";
    private const string EndAssignment = "EndAssignment";
    private const string GetLoanOfficerAssignment = "GetLoanOfficerAssignment";
    private const string TransferAssignment = "TransferAssignment";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/loan-officer-assignments").WithTags("Loan Officer Assignments");

        group.MapPost("/member", async (AssignToMemberCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/loan-officer-assignments/{result.Id}", result);
        })
        .WithName(AssignToMember)
        .WithSummary("Assign loan officer to a member")
        .Produces<AssignToMemberResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/group", async (AssignToGroupCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/loan-officer-assignments/{result.Id}", result);
        })
        .WithName(AssignToGroup)
        .WithSummary("Assign loan officer to a group")
        .Produces<AssignToGroupResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetLoanOfficerAssignmentRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetLoanOfficerAssignment)
        .WithSummary("Get loan officer assignment by ID")
        .Produces<LoanOfficerAssignmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/transfer", async (Guid id, TransferAssignmentRequest request, ISender sender) =>
        {
            var command = new TransferAssignmentCommand(id, request.NewStaffId, request.Reason);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(TransferAssignment)
        .WithSummary("Transfer assignment to another loan officer")
        .Produces<TransferAssignmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Transfer, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/end", async (Guid id, EndAssignmentRequest request, ISender sender) =>
        {
            var command = new EndAssignmentCommand(id, request.EndDate, request.Reason);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName(EndAssignment)
        .WithSummary("End loan officer assignment")
        .Produces<EndAssignmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public sealed record TransferAssignmentRequest(Guid NewStaffId, string? Reason);
public sealed record EndAssignmentRequest(DateOnly? EndDate, string? Reason);
