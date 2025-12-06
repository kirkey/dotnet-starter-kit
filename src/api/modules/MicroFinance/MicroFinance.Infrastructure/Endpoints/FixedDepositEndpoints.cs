using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.ClosePremature.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Mature.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.PayInterest.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.PostInterest.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Renew.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.UpdateInstruction.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Fixed Deposits.
/// </summary>
public class FixedDepositEndpoints() : CarterModule("microfinance")
{

    private const string CloseFixedDepositPremature = "CloseFixedDepositPremature";
    private const string CreateFixedDeposit = "CreateFixedDeposit";
    private const string GetFixedDeposit = "GetFixedDeposit";
    private const string MatureFixedDeposit = "MatureFixedDeposit";
    private const string PayFixedDepositInterest = "PayFixedDepositInterest";
    private const string PostFixedDepositInterest = "PostFixedDepositInterest";
    private const string RenewFixedDeposit = "RenewFixedDeposit";
    private const string SearchFixedDeposits = "SearchFixedDeposits";
    private const string UpdateMaturityInstruction = "UpdateMaturityInstruction";

    /// <summary>
    /// Maps all Fixed Deposit endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var fixedDepositsGroup = app.MapGroup("microfinance/fixed-deposits").WithTags("fixed-deposits");

        fixedDepositsGroup.MapPost("/", async (CreateFixedDepositCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Created($"/api/microfinance/fixed-deposits/{response}", new { id = response });
        })
        .WithName(CreateFixedDeposit)
        .WithSummary("Creates a new fixed deposit")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        fixedDepositsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetFixedDepositRequest(id));
            return Results.Ok(response);
        })
        .WithName(GetFixedDeposit)
        .WithSummary("Gets a fixed deposit by ID")
        .Produces<FixedDepositResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        fixedDepositsGroup.MapPost("/search", async (SearchFixedDepositsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(SearchFixedDeposits)
        .WithSummary("Searches fixed deposits with pagination")
        .Produces<PagedList<FixedDepositResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
        .MapToApiVersion(1);

        fixedDepositsGroup.MapPost("/{id:guid}/post-interest", async (Guid id, PostFixedDepositInterestCommand command, ISender mediator) =>
        {
            if (id != command.DepositId) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(PostFixedDepositInterest)
        .WithSummary("Posts interest to a fixed deposit")
        .Produces<PostFixedDepositInterestResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.MicroFinance))
        .MapToApiVersion(1);

        fixedDepositsGroup.MapPost("/{id:guid}/pay-interest", async (Guid id, PayFixedDepositInterestCommand command, ISender mediator) =>
        {
            if (id != command.DepositId) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(PayFixedDepositInterest)
        .WithSummary("Pays accrued interest on a fixed deposit")
        .Produces<PayFixedDepositInterestResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.MicroFinance))
        .MapToApiVersion(1);

        fixedDepositsGroup.MapPost("/{id:guid}/mature", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new MatureFixedDepositCommand(id));
            return Results.Ok(response);
        })
        .WithName(MatureFixedDeposit)
        .WithSummary("Matures a fixed deposit")
        .Produces<MatureFixedDepositResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Mature, FshResources.MicroFinance))
        .MapToApiVersion(1);

        fixedDepositsGroup.MapPost("/{id:guid}/renew", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new RenewFixedDepositCommand(id));
            return Results.Ok(response);
        })
        .WithName(RenewFixedDeposit)
        .WithSummary("Renews a matured fixed deposit")
        .Produces<RenewFixedDepositResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Renew, FshResources.MicroFinance))
        .MapToApiVersion(1);

        fixedDepositsGroup.MapPost("/{id:guid}/close-premature", async (Guid id, ClosePrematureFixedDepositCommand command, ISender mediator) =>
        {
            if (id != command.DepositId) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(CloseFixedDepositPremature)
        .WithSummary("Closes a fixed deposit prematurely")
        .Produces<ClosePrematureFixedDepositResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Close, FshResources.MicroFinance))
        .MapToApiVersion(1);

        fixedDepositsGroup.MapPut("/{id:guid}/maturity-instruction", async (Guid id, UpdateMaturityInstructionCommand command, ISender mediator) =>
        {
            if (id != command.DepositId) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(UpdateMaturityInstruction)
        .WithSummary("Updates the maturity instruction for a fixed deposit")
        .Produces<UpdateMaturityInstructionResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
