using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Search.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Fixed Deposits.
/// </summary>
public static class FixedDepositEndpoints
{
    /// <summary>
    /// Maps all Fixed Deposit endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapFixedDepositEndpoints(this IEndpointRouteBuilder app)
    {
        var fixedDepositsGroup = app.MapGroup("fixed-deposits").WithTags("fixed-deposits");

        fixedDepositsGroup.MapPost("/", async (CreateFixedDepositCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Created($"/api/microfinance/fixed-deposits/{response}", new { id = response });
        })
        .WithName("CreateFixedDeposit")
        .WithSummary("Creates a new fixed deposit")
        .Produces<Guid>(StatusCodes.Status201Created);

        fixedDepositsGroup.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetFixedDepositRequest(id));
            return Results.Ok(response);
        })
        .WithName("GetFixedDeposit")
        .WithSummary("Gets a fixed deposit by ID")
        .Produces<FixedDepositResponse>();

        fixedDepositsGroup.MapPost("/search", async (SearchFixedDepositsCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        })
        .WithName("SearchFixedDeposits")
        .WithSummary("Searches fixed deposits with pagination")
        .Produces<PagedList<FixedDepositResponse>>();

        return app;
    }
}
