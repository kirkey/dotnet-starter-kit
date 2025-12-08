using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Approve.v1;
using FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.RecordPayment.v1;
using FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.Search.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class DebtSettlementEndpoints : CarterModule
{

    private const string ApproveSettlement = "ApproveSettlement";
    private const string CreateDebtSettlement = "CreateDebtSettlement";
    private const string GetDebtSettlement = "GetDebtSettlement";
    private const string RecordSettlementPayment = "RecordSettlementPayment";
    private const string SearchDebtSettlements = "SearchDebtSettlements";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/debt-settlements").WithTags("Debt Settlements");

        group.MapPost("/", async (CreateDebtSettlementCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/debt-settlements/{result.Id}", result);
        })
        .WithName(CreateDebtSettlement)
        .WithSummary("Create a new debt settlement")
        .Produces<CreateDebtSettlementResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetDebtSettlementRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetDebtSettlement)
        .WithSummary("Get debt settlement by ID")
        .Produces<DebtSettlementResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveSettlementRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ApproveSettlementCommand(id, request.ApprovedById));
            return Results.Ok(result);
        })
        .WithName(ApproveSettlement)
        .WithSummary("Approve a debt settlement")
        .Produces<ApproveSettlementResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/record-payment", async (DefaultIdType id, SettlementPaymentRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RecordSettlementPaymentCommand(id, request.Amount));
            return Results.Ok(result);
        })
        .WithName(RecordSettlementPayment)
        .WithSummary("Record payment for a debt settlement")
        .Produces<RecordSettlementPaymentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchDebtSettlementsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchDebtSettlements)
        .WithSummary("Search debt settlements")
        .Produces<PagedList<DebtSettlementSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record ApproveSettlementRequest(DefaultIdType ApprovedById);
public record SettlementPaymentRequest(decimal Amount);
