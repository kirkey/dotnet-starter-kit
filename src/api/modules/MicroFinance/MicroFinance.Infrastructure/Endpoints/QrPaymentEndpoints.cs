using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.QrPayments.CreateDynamic.v1;
using FSH.Starter.WebApi.MicroFinance.Application.QrPayments.CreateStatic.v1;
using FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for QR Payments.
/// </summary>
public class QrPaymentEndpoints : CarterModule
{

    private const string CreateDynamicQr = "CreateDynamicQr";
    private const string CreateStaticQr = "CreateStaticQr";
    private const string GetQrPayment = "GetQrPayment";

    /// <summary>
    /// Maps all QR Payment endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/qr-payments").WithTags("QR Payments");

        group.MapPost("/static", async (CreateStaticQrCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/qr-payments/{response.Id}", response);
            })
            .WithName(CreateStaticQr)
            .WithSummary("Creates a static QR code for payments")
            .Produces<CreateStaticQrResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/dynamic", async (CreateDynamicQrCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/qr-payments/{response.Id}", response);
            })
            .WithName(CreateDynamicQr)
            .WithSummary("Creates a dynamic QR code with amount and expiry")
            .Produces<CreateDynamicQrResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetQrPaymentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetQrPayment)
            .WithSummary("Gets a QR payment by ID")
            .Produces<QrPaymentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
