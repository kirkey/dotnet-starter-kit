using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.MarkBroken.v1;
using FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.RecordPayment.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class PromiseToPayEndpoints : CarterModule
{

    private const string CreatePromiseToPay = "CreatePromiseToPay";
    private const string GetPromiseToPay = "GetPromiseToPay";
    private const string MarkPromiseBroken = "MarkPromiseBroken";
    private const string RecordPromisePayment = "RecordPromisePayment";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/promise-to-pays").WithTags("Promise to Pays");

        group.MapPost("/", async (CreatePromiseToPayCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/promise-to-pays/{result.Id}", result);
        })
        .WithName(CreatePromiseToPay)
        .WithSummary("Create a new promise to pay")
        .Produces<CreatePromiseToPayResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetPromiseToPayRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetPromiseToPay)
        .WithSummary("Get promise to pay by ID")
        .Produces<PromiseToPayResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/record-payment", async (DefaultIdType id, RecordPaymentRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RecordPromisePaymentCommand(id, request.Amount, request.PaymentDate));
            return Results.Ok(result);
        })
        .WithName(RecordPromisePayment)
        .WithSummary("Record payment against a promise")
        .Produces<RecordPromisePaymentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/mark-broken", async (DefaultIdType id, MarkBrokenRequest request, ISender sender) =>
        {
            var result = await sender.Send(new MarkPromiseBrokenCommand(id, request.Reason));
            return Results.Ok(result);
        })
        .WithName(MarkPromiseBroken)
        .WithSummary("Mark a promise as broken")
        .Produces<MarkPromiseBrokenResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record RecordPaymentRequest(decimal Amount, DateOnly PaymentDate);
public record MarkBrokenRequest(string Reason);
