using Accounting.Application.TaxCodes.Create.v1;
using Accounting.Application.TaxCodes.Delete.v1;
using Accounting.Application.TaxCodes.Get.v1;
using Accounting.Application.TaxCodes.Responses;
using Accounting.Application.TaxCodes.Search.v1;
using Accounting.Application.TaxCodes.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.TaxCodes;

/// <summary>
/// Endpoint configuration for Tax Codes module.
/// </summary>
public class TaxCodesEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/tax-codes").WithTags("tax-codes");

        group.MapPost("/", async (CreateTaxCodeCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreateTaxCode")
        .WithSummary("Create a tax code")
        .WithDescription("Create a new tax code with rate and jurisdiction")
        .Produces<DefaultIdType>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetTaxCodeRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetTaxCode")
        .WithSummary("Get a tax code")
        .WithDescription("Get a tax code by ID")
        .Produces<TaxCodeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateTaxCodeCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("UpdateTaxCode")
        .WithSummary("Update a tax code")
        .WithDescription("Update tax code information (non-rate fields)")
        .Produces<UpdateTaxCodeResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            await mediator.Send(new DeleteTaxCodeCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteTaxCode")
        .WithSummary("Delete a tax code")
        .WithDescription("Delete a tax code by ID")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchTaxCodesCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchTaxCodes")
        .WithSummary("Search tax codes")
        .WithDescription("Search and filter tax codes with pagination")
        .Produces<PagedList<TaxCodeResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}
