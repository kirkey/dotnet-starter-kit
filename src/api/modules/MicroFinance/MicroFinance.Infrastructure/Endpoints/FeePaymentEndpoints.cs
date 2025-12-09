using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Reverse.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class FeePaymentEndpoints : CarterModule
{
    private const string CreateFeePayment = "CreateFeePayment";
    private const string GetFeePayment = "GetFeePayment";
    private const string SearchFeePayments = "SearchFeePayments";
    private const string UpdateFeePayment = "UpdateFeePayment";
    private const string ReverseFeePayment = "ReverseFeePayment";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/fee-payments").WithTags("Fee Payments");

        group.MapPost("/", async (CreateFeePaymentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/fee-payments/{result.Id}", result);
        })
        .WithName(CreateFeePayment)
        .WithSummary("Create a new fee payment")
        .Produces<CreateFeePaymentResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetFeePaymentRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetFeePayment)
        .WithSummary("Get fee payment by ID")
        .Produces<FeePaymentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateFeePaymentCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(UpdateFeePayment)
        .WithSummary("Update a fee payment")
        .Produces<UpdateFeePaymentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reverse", async (DefaultIdType id, ReverseFeePaymentCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(ReverseFeePayment)
        .WithSummary("Reverse a fee payment")
        .Produces<ReverseFeePaymentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchFeePaymentsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(SearchFeePayments)
        .WithSummary("Search fee payments")
        .Produces<PagedList<FeePaymentSummaryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);
    }
}
