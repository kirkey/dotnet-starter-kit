using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Search.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Fee Charges.
/// </summary>
public static class FeeChargeEndpoints
{
    /// <summary>
    /// Maps all Fee Charge endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapFeeChargeEndpoints(this IEndpointRouteBuilder app)
    {
        var chargesGroup = app.MapGroup("fee-charges").WithTags("fee-charges");

        chargesGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetFeeChargeRequest(id));
            return Results.Ok(response);
        })
        .WithName("GetFeeCharge")
        .WithSummary("Gets a fee charge by ID")
        .Produces<FeeChargeResponse>();

        chargesGroup.MapPost("/search", async (SearchFeeChargesCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("SearchFeeCharges")
        .WithSummary("Searches fee charges with pagination")
        .Produces<PagedList<FeeChargeResponse>>();

        chargesGroup.MapGet("/by-member/{memberId:guid}", async (Guid memberId, ISender mediator) =>
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
        .WithName("GetFeeChargesByMember")
        .WithSummary("Gets all fee charges for a member")
        .Produces<PagedList<FeeChargeResponse>>();

        chargesGroup.MapGet("/by-definition/{feeDefinitionId:guid}", async (Guid feeDefinitionId, ISender mediator) =>
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
        .WithName("GetFeeChargesByDefinition")
        .WithSummary("Gets all fee charges for a fee definition")
        .Produces<PagedList<FeeChargeResponse>>();

        return app;
    }
}
