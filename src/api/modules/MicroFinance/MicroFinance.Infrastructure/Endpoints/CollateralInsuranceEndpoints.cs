using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.RecordPremium.v1;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Renew.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class CollateralInsuranceEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/collateral-insurances").WithTags("Collateral Insurances");

        group.MapPost("/", async (CreateCollateralInsuranceCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/collateral-insurances/{result.Id}", result);
        })
        .WithName("CreateCollateralInsurance")
        .WithSummary("Create a new collateral insurance")
        .Produces<CreateCollateralInsuranceResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCollateralInsuranceRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetCollateralInsurance")
        .WithSummary("Get collateral insurance by ID")
        .Produces<CollateralInsuranceResponse>();

        group.MapPost("/{id:guid}/record-premium", async (Guid id, RecordPremiumRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RecordPremiumPaymentCommand(id, request.PaymentDate, request.NextDueDate));
            return Results.Ok(result);
        })
        .WithName("RecordPremiumPayment")
        .WithSummary("Record insurance premium payment")
        .Produces<RecordPremiumPaymentResponse>();

        group.MapPost("/{id:guid}/renew", async (Guid id, RenewRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RenewInsuranceCommand(id, request.NewExpiryDate, request.NewPremium));
            return Results.Ok(result);
        })
        .WithName("RenewInsurance")
        .WithSummary("Renew collateral insurance")
        .Produces<RenewInsuranceResponse>();

    }
}

public record RecordPremiumRequest(DateOnly PaymentDate, DateOnly NextDueDate);
public record RenewRequest(DateOnly NewExpiryDate, decimal? NewPremium);
