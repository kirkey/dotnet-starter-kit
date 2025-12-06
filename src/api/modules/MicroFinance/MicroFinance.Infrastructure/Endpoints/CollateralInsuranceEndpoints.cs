using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.RecordPremium.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Renew.v1;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollateralInsuranceEndpoints() : CarterModule("microfinance")
{

    private const string CreateCollateralInsurance = "CreateCollateralInsurance";
    private const string GetCollateralInsurance = "GetCollateralInsurance";
    private const string RecordPremiumPayment = "RecordPremiumPayment";
    private const string RenewInsurance = "RenewInsurance";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collateral-insurances").WithTags("Collateral Insurances");

        group.MapPost("/", async (CreateCollateralInsuranceCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collateral-insurances/{result.Id}", result);
        })
        .WithName(CreateCollateralInsurance)
        .WithSummary("Create a new collateral insurance")
        .Produces<CreateCollateralInsuranceResponse>(StatusCodes.Status201Created)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollateralInsuranceRequest(id));
            return Results.Ok(result);
        })
        .WithName(GetCollateralInsurance)
        .WithSummary("Get collateral insurance by ID")
        .Produces<CollateralInsuranceResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/record-premium", async (Guid id, RecordPremiumRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RecordPremiumPaymentCommand(id, request.PaymentDate, request.NextDueDate));
            return Results.Ok(result);
        })
        .WithName(RecordPremiumPayment)
        .WithSummary("Record insurance premium payment")
        .Produces<RecordPremiumPaymentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.MicroFinance))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/renew", async (Guid id, RenewRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RenewInsuranceCommand(id, request.NewExpiryDate, request.NewPremium));
            return Results.Ok(result);
        })
        .WithName(RenewInsurance)
        .WithSummary("Renew collateral insurance")
        .Produces<RenewInsuranceResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Renew, FshResources.MicroFinance))
        .MapToApiVersion(1);

    }
}

public record RecordPremiumRequest(DateOnly PaymentDate, DateOnly NextDueDate);
public record RenewRequest(DateOnly NewExpiryDate, decimal? NewPremium);
