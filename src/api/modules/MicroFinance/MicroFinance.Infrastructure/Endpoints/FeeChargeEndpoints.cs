using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.RecordPayment.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Reverse.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Waive.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Fee Charges.
/// </summary>
public class FeeChargeEndpoints : CarterModule
{

    private const string CreateFeeCharge = "CreateFeeCharge";
    private const string GetFeeCharge = "GetFeeCharge";
    private const string GetFeeChargesByDefinition = "GetFeeChargesByDefinition";
    private const string GetFeeChargesByMember = "GetFeeChargesByMember";
    private const string RecordFeePayment = "RecordFeePayment";
    private const string ReverseFeeCharge = "ReverseFeeCharge";
    private const string SearchFeeCharges = "SearchFeeCharges";
    private const string WaiveFeeCharge = "WaiveFeeCharge";

    /// <summary>
    /// Maps all Fee Charge endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var chargesGroup = app.MapGroup("microfinance/fee-charges").WithTags("Fee Charges");

        chargesGroup.MapPost("/", async (CreateFeeChargeCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command);
            return Results.Created($"/microfinance/fee-charges/{response.Id}", response);
        })
        .WithName(CreateFeeCharge)
        .WithSummary("Creates a new fee charge")
        .Produces<CreateFeeChargeResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        chargesGroup.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetFeeChargeRequest(id));
            return Results.Ok(response);
        })
        .WithName(GetFeeCharge)
        .WithSummary("Gets a fee charge by ID")
        .Produces<FeeChargeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        chargesGroup.MapPost("/search", async (SearchFeeChargesCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(SearchFeeCharges)
        .WithSummary("Searches fee charges with pagination")
        .Produces<PagedList<FeeChargeResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
        .MapToApiVersion(1);

        chargesGroup.MapGet("/by-member/{memberId:guid}", async (DefaultIdType memberId, ISender mediator) =>
        {
            var request = new SearchFeeChargesCommand
            {
                MemberId = memberId,
                PageNumber = 1,
                PageSize = 100
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(GetFeeChargesByMember)
        .WithSummary("Gets all fee charges for a member")
        .Produces<PagedList<FeeChargeResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        chargesGroup.MapGet("/by-definition/{feeDefinitionId:guid}", async (DefaultIdType feeDefinitionId, ISender mediator) =>
        {
            var request = new SearchFeeChargesCommand
            {
                FeeDefinitionId = feeDefinitionId,
                PageNumber = 1,
                PageSize = 100
            };
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName(GetFeeChargesByDefinition)
        .WithSummary("Gets all fee charges for a fee definition")
        .Produces<PagedList<FeeChargeResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        chargesGroup.MapPost("/{id}/payment", async (DefaultIdType id, RecordFeePaymentCommand command, ISender mediator) =>
        {
            if (id != command.FeeChargeId) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(RecordFeePayment)
        .WithSummary("Records a payment for a fee charge")
        .Produces<RecordFeePaymentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        chargesGroup.MapPost("/{id}/waive", async (DefaultIdType id, WaiveFeeChargeCommand command, ISender mediator) =>
        {
            if (id != command.FeeChargeId) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(WaiveFeeCharge)
        .WithSummary("Waives a fee charge")
        .Produces<WaiveFeeChargeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        chargesGroup.MapPost("/{id}/reverse", async (DefaultIdType id, ReverseFeeChargeCommand command, ISender mediator) =>
        {
            if (id != command.FeeChargeId) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(ReverseFeeCharge)
        .WithSummary("Reverses a fee charge")
        .Produces<ReverseFeeChargeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}
