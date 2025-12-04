using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;
using Accounting.Application.InterCompanyTransactions.Create.v1;
using Accounting.Application.InterCompanyTransactions.Get;
using Accounting.Application.InterCompanyTransactions.Responses;
using Accounting.Application.InterCompanyTransactions.Search.v1;

namespace Accounting.Infrastructure.Endpoints.InterCompanyTransactions;

public class InterCompanyTransactionsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/intercompany-transactions").WithTags("intercompany-transactions");

        group.MapPost("/", async (InterCompanyTransactionCreateCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Created($"/accounting/intercompany-transactions/{response.Id}", response);
        })
        .WithName("CreateInterCompanyTransaction")
        .WithSummary("Create inter-company transaction")
        .Produces<InterCompanyTransactionCreateResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetInterCompanyTransactionRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetInterCompanyTransaction")
        .WithSummary("Get inter-company transaction by ID")
        .WithDescription("Retrieves an inter-company transaction by its unique identifier")
        .Produces<InterCompanyTransactionResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchInterCompanyTransactionsRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchInterCompanyTransactions")
        .WithSummary("Search inter-company transactions")
        .Produces<PagedList<InterCompanyTransactionResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}

