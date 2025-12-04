using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.ClosePremature.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Mature.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.PayInterest.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.PostInterest.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Renew.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.UpdateInstruction.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Fixed Deposits.
/// </summary>
public class FixedDepositEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Fixed Deposit endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var fixedDepositsGroup = app.MapGroup("microfinance/fixed-deposits").WithTags("fixed-deposits");

        fixedDepositsGroup.MapPost("/", async (CreateFixedDepositCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Created($"/api/microfinance/fixed-deposits/{response}", new { id = response });
        })
        .WithName("CreateFixedDeposit")
        .WithSummary("Creates a new fixed deposit")
        .Produces<Guid>(StatusCodes.Status201Created);

        fixedDepositsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetFixedDepositRequest(id));
            return Results.Ok(response);
        })
        .WithName("GetFixedDeposit")
        .WithSummary("Gets a fixed deposit by ID")
        .Produces<FixedDepositResponse>();

        fixedDepositsGroup.MapPost("/search", async (SearchFixedDepositsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("SearchFixedDeposits")
        .WithSummary("Searches fixed deposits with pagination")
        .Produces<PagedList<FixedDepositResponse>>();

        fixedDepositsGroup.MapPost("/{id:guid}/post-interest", async (Guid id, PostFixedDepositInterestCommand command, ISender mediator) =>
        {
            if (id != command.DepositId) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName("PostFixedDepositInterest")
        .WithSummary("Posts interest to a fixed deposit")
        .Produces<PostFixedDepositInterestResponse>();

        fixedDepositsGroup.MapPost("/{id:guid}/pay-interest", async (Guid id, PayFixedDepositInterestCommand command, ISender mediator) =>
        {
            if (id != command.DepositId) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName("PayFixedDepositInterest")
        .WithSummary("Pays accrued interest on a fixed deposit")
        .Produces<PayFixedDepositInterestResponse>();

        fixedDepositsGroup.MapPost("/{id:guid}/mature", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new MatureFixedDepositCommand(id));
            return Results.Ok(response);
        })
        .WithName("MatureFixedDeposit")
        .WithSummary("Matures a fixed deposit")
        .Produces<MatureFixedDepositResponse>();

        fixedDepositsGroup.MapPost("/{id:guid}/renew", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new RenewFixedDepositCommand(id));
            return Results.Ok(response);
        })
        .WithName("RenewFixedDeposit")
        .WithSummary("Renews a matured fixed deposit")
        .Produces<RenewFixedDepositResponse>();

        fixedDepositsGroup.MapPost("/{id:guid}/close-premature", async (Guid id, ClosePrematureFixedDepositCommand command, ISender mediator) =>
        {
            if (id != command.DepositId) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName("CloseFixedDepositPremature")
        .WithSummary("Closes a fixed deposit prematurely")
        .Produces<ClosePrematureFixedDepositResponse>();

        fixedDepositsGroup.MapPut("/{id:guid}/maturity-instruction", async (Guid id, UpdateMaturityInstructionCommand command, ISender mediator) =>
        {
            if (id != command.DepositId) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName("UpdateMaturityInstruction")
        .WithSummary("Updates the maturity instruction for a fixed deposit")
        .Produces<UpdateMaturityInstructionResponse>();

    }
}
