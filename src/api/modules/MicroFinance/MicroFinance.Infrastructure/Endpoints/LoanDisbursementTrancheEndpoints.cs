using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Disburse.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.VerifyMilestone.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class LoanDisbursementTrancheEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/loan-disbursement-tranches").WithTags("Loan Disbursement Tranches");

        group.MapPost("/", async (CreateLoanDisbursementTrancheCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/loan-disbursement-tranches/{result.Id}", result);
        })
        .WithName("CreateLoanDisbursementTranche")
        .WithSummary("Create a new loan disbursement tranche")
        .Produces<CreateLoanDisbursementTrancheResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetLoanDisbursementTrancheRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetLoanDisbursementTranche")
        .WithSummary("Get loan disbursement tranche by ID")
        .Produces<LoanDisbursementTrancheResponse>();

        group.MapPost("/{id:guid}/verify-milestone", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new VerifyMilestoneCommand(id));
            return Results.Ok(result);
        })
        .WithName("VerifyTrancheMilestone")
        .WithSummary("Verify tranche milestone")
        .Produces<VerifyMilestoneResponse>();

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveTrancheRequest request, ISender sender) =>
        {
            var command = new ApproveTrancheCommand(id, request.UserId);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("ApproveTranche")
        .WithSummary("Approve disbursement tranche")
        .Produces<ApproveTrancheResponse>();

        group.MapPost("/{id:guid}/disburse", async (Guid id, DisburseTrancheRequest request, ISender sender) =>
        {
            var command = new DisburseTrancheCommand(id, request.UserId, request.ReferenceNumber, request.DisbursedDate);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("DisburseTranche")
        .WithSummary("Disburse tranche")
        .Produces<DisburseTrancheResponse>();

    }
}

public sealed record ApproveTrancheRequest(Guid UserId);
public sealed record DisburseTrancheRequest(Guid UserId, string ReferenceNumber, DateOnly? DisbursedDate);
