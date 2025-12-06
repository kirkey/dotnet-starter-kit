using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Get.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Payment Gateways.
/// </summary>
public class PaymentGatewayEndpoints() : CarterModule("microfinance")
{

    private const string ActivatePaymentGateway = "ActivatePaymentGateway";
    private const string CreatePaymentGateway = "CreatePaymentGateway";
    private const string GetPaymentGateway = "GetPaymentGateway";

    /// <summary>
    /// Maps all Payment Gateway endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/payment-gateways").WithTags("payment-gateways");

        group.MapPost("/", async (CreatePaymentGatewayCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Created($"/microfinance/payment-gateways/{response.Id}", response);
            })
            .WithName(CreatePaymentGateway)
            .WithSummary("Creates a new payment gateway configuration")
            .Produces<CreatePaymentGatewayResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new GetPaymentGatewayRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetPaymentGateway)
            .WithSummary("Gets a payment gateway by ID")
            .Produces<PaymentGatewayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id:guid}/activate", async (Guid id, ISender sender) =>
            {
                var response = await sender.Send(new ActivatePaymentGatewayCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ActivatePaymentGateway)
            .WithSummary("Activates a payment gateway")
            .Produces<ActivatePaymentGatewayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
