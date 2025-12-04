using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class LoanRestructureEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/loan-restructures").WithTags("Loan Restructures");

        group.MapPost("/", async (CreateLoanRestructureCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/loan-restructures/{result.Id}", result);
        })
        .WithName("CreateLoanRestructure")
        .WithSummary("Create a new loan restructure")
        .Produces<CreateLoanRestructureResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetLoanRestructureRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetLoanRestructure")
        .WithSummary("Get loan restructure by ID")
        .Produces<LoanRestructureResponse>();

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveRestructureRequest request, ISender sender) =>
        {
            var command = new ApproveRestructureCommand(id, request.UserId, request.ApproverName, request.EffectiveDate);
            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("ApproveRestructure")
        .WithSummary("Approve loan restructure")
        .Produces<ApproveRestructureResponse>();

    }
}

public sealed record ApproveRestructureRequest(Guid UserId, string ApproverName, DateOnly EffectiveDate);
