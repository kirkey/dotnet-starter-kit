using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Reject.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Submit.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollateralValuationEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collateral-valuations").WithTags("Collateral Valuations");

        group.MapPost("/", async (CreateCollateralValuationCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collateral-valuations/{result.Id}", result);
        })
        .WithName("CreateCollateralValuation")
        .WithSummary("Create a new collateral valuation")
        .Produces<CreateCollateralValuationResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollateralValuationRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetCollateralValuation")
        .WithSummary("Get collateral valuation by ID")
        .Produces<CollateralValuationResponse>();

        group.MapPost("/{id:guid}/submit", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new SubmitValuationCommand(id));
            return Results.Ok(result);
        })
        .WithName("SubmitValuation")
        .WithSummary("Submit valuation for approval")
        .Produces<SubmitValuationResponse>();

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ApproveValuationCommand(id, request.ApprovedById));
            return Results.Ok(result);
        })
        .WithName("ApproveValuation")
        .WithSummary("Approve a collateral valuation")
        .Produces<ApproveValuationResponse>();

        group.MapPost("/{id:guid}/reject", async (Guid id, RejectRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RejectValuationCommand(id, request.Reason));
            return Results.Ok(result);
        })
        .WithName("RejectValuation")
        .WithSummary("Reject a collateral valuation")
        .Produces<RejectValuationResponse>();

    }
}

public record ApproveRequest(Guid ApprovedById);
public record RejectRequest(string Reason);
