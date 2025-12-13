using Carter;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Activate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Deactivate.v1;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Search.v1;
using FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Update.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Payment Gateways.
/// </summary>
public class PaymentGatewayEndpoints : CarterModule
{
    private const string ActivatePaymentGateway = "ActivatePaymentGateway";
    private const string DeactivatePaymentGateway = "DeactivatePaymentGateway";
    private const string CreatePaymentGateway = "CreatePaymentGateway";
    private const string GetPaymentGateway = "GetPaymentGateway";
    private const string SearchPaymentGateways = "SearchPaymentGateways";
    private const string UpdatePaymentGateway = "UpdatePaymentGateway";

    /// <summary>
    /// Maps all Payment Gateway endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/payment-gateways").WithTags("Payment Gateways");

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

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetPaymentGatewayRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(GetPaymentGateway)
            .WithSummary("Gets a payment gateway by ID")
            .Produces<PaymentGatewayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/search", async ([FromBody] SearchPaymentGatewaysCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(SearchPaymentGateways)
            .WithSummary("Searches payment gateways with filters and pagination")
            .Produces<PagedList<PaymentGatewayResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id}/activate", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new ActivatePaymentGatewayCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(ActivatePaymentGateway)
            .WithSummary("Activates a payment gateway")
            .Produces<ActivatePaymentGatewayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Activate, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPost("/{id}/deactivate", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new DeactivatePaymentGatewayCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(DeactivatePaymentGateway)
            .WithSummary("Deactivates a payment gateway")
            .Produces<DeactivatePaymentGatewayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Deactivate, FshResources.MicroFinance))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePaymentGatewayCommand command, ISender sender) =>
            {
                if (id != command.Id) return Results.BadRequest("ID mismatch");
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(UpdatePaymentGateway)
            .WithSummary("Updates a payment gateway configuration")
            .Produces<UpdatePaymentGatewayResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.MicroFinance))
            .MapToApiVersion(1);
    }
}
