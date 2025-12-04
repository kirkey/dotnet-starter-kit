using Accounting.Application.Banks.Create.v1;
using Accounting.Application.Banks.Delete.v1;
using Accounting.Application.Banks.Get.v1;
using Accounting.Application.Banks.Search.v1;
using Accounting.Application.Banks.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Banks;

/// <summary>
/// Endpoint configuration for Banks module.
/// Provides comprehensive REST API endpoints for managing bank accounts and information.
/// </summary>
public class BanksEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Banks endpoints to the route builder.
    /// Includes Create, Read, Update, Delete, and Search operations for banks.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/banks").WithTags("banks");

        // Create endpoint
        group.MapPost("/", async (BankCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreateBank")
            .WithSummary("Create a new bank")
            .WithDescription("Creates a new bank in the accounting system with comprehensive validation and returns the created bank ID.")
            .Produces<BankCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new BankGetRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetBank")
            .WithSummary("Get a bank by ID")
            .WithDescription("Retrieves a bank by its unique identifier with all details.")
            .Produces<BankResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update endpoint
        group.MapPut("/{id}", async (DefaultIdType id, BankUpdateCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateBank")
            .WithSummary("Update an existing bank")
            .WithDescription("Updates an existing bank in the accounting system with comprehensive validation.")
            .Produces<BankUpdateResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Delete endpoint
        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new BankDeleteCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("DeleteBank")
            .WithSummary("Delete a bank")
            .WithDescription("Deletes a bank from the accounting system. Consider deactivating instead of deleting to preserve historical data.")
            .Produces<BankDeleteResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        // Search endpoint
        group.MapPost("/search", async (BankSearchRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchBanks")
            .WithSummary("Search banks")
            .WithDescription("Searches banks with filtering by bank code, name, routing number, SWIFT code, and active status. Supports pagination and sorting.")
            .Produces<PagedList<BankResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));
    }
}

