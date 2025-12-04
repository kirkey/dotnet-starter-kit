using Carter;
using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TaxBrackets;

/// <summary>
/// Endpoint routes for managing tax brackets.
/// </summary>
public class TaxBracketEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Tax Bracket endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/tax-brackets").WithTags("tax-brackets");

        group.MapPost("/", async (CreateTaxBracketCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreateTaxBracket")
            .WithSummary("Create tax bracket")
            .WithDescription("Creates new tax bracket for income taxation")
            .Produces<CreateTaxBracketResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateTaxBracketCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateTaxBracket")
            .WithSummary("Update tax bracket")
            .WithDescription("Updates tax bracket details")
            .Produces<UpdateTaxBracketResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetTaxBracketRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetTaxBracket")
            .WithSummary("Get tax bracket by ID")
            .WithDescription("Retrieves tax bracket by its unique identifier")
            .Produces<TaxBracketResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteTaxBracketCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteTaxBracket")
            .WithSummary("Delete tax bracket")
            .WithDescription("Deletes tax bracket by its unique identifier")
            .Produces<DeleteTaxBracketResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchTaxBracketsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchTaxBrackets")
            .WithSummary("Searches tax brackets")
            .WithDescription("Searches and filters tax brackets by type, year, filing status, and income range with pagination support.")
            .Produces<PagedList<TaxBracketResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}


