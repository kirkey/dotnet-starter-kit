using Carter;
using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates;

/// <summary>
/// Endpoint configuration for PayComponentRates module.
/// </summary>
public class PayComponentRateEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all PayComponentRates endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/pay-component-rates").WithTags("pay-component-rates");

        group.MapPost("/", async (CreatePayComponentRateCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/hr/pay-component-rates/{response.Id}", response);
            })
            .WithName("CreatePayComponentRate")
            .WithSummary("Create a new pay component rate")
            .WithDescription("Creates a new rate/bracket for pay component")
            .Produces<CreatePayComponentRateResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPayComponentRateRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPayComponentRate")
            .WithSummary("Get a pay component rate by ID")
            .WithDescription("Retrieves a specific pay component rate/bracket by its unique identifier")
            .Produces<PayComponentRateResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePayComponentRateCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdatePayComponentRate")
            .WithSummary("Update a pay component rate")
            .WithDescription("Updates an existing pay component rate/bracket")
            .Produces<UpdatePayComponentRateResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeletePayComponentRateCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeletePayComponentRate")
            .WithSummary("Delete a pay component rate")
            .WithDescription("Deletes a pay component rate/bracket by its unique identifier")
            .Produces<DeletePayComponentRateResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchPayComponentRatesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPayComponentRates")
            .WithSummary("Searches pay component rates")
            .WithDescription("Searches and filters pay component rates (tax brackets, SSS rates, etc.) by component, year, amount range with pagination support.")
            .Produces<PagedList<PayComponentRateResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

