using Accounting.Application.Payees.Create.v1;
using Accounting.Application.Payees.Delete.v1;
using Accounting.Application.Payees.Export.v1;
using Accounting.Application.Payees.Get.v1;
using Accounting.Application.Payees.Import.v1;
using Accounting.Application.Payees.Search.v1;
using Accounting.Application.Payees.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Payees;

/// <summary>
/// Endpoint configuration for Payees module.
/// Provides comprehensive REST API endpoints for managing payees.
/// </summary>
public class PayeesEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Payees endpoints to the route builder.
    /// Includes Create, Read, Update, Delete, Search, Import, and Export operations for payees.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/payees").WithTags("payees");

        // Create endpoint
        group.MapPost("/", async (PayeeCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("CreatePayee")
            .WithSummary("Create a new payee")
            .WithDescription("Creates a new payee in the accounting system with comprehensive validation and returns the created payee ID.")
            .Produces<PayeeCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new PayeeGetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPayee")
            .WithSummary("Get a payee by ID")
            .WithDescription("Retrieves a specific payee from the accounting system using its unique identifier.")
            .Produces<PayeeResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, PayeeUpdateCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID in URL must match ID in request body.");

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdatePayee")
            .WithSummary("Update an existing payee")
            .WithDescription("Updates an existing payee in the accounting system with comprehensive validation.")
            .Produces<PayeeUpdateResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Delete endpoint
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new PayeeDeleteCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeletePayee")
            .WithSummary("Delete a payee")
            .WithDescription("Deletes a payee from the accounting system. Returns 204 No Content on successful deletion.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        // Search endpoint
        group.MapPost("/search", async (PayeeSearchCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPayees")
            .WithSummary("Search payees with pagination")
            .WithDescription("Searches payees with comprehensive filtering capabilities including keyword search, payee code, name, expense account code, and TIN filters with pagination support.")
            .Produces<PagedList<PayeeResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Import endpoint
        group.MapPost("/import", async (ImportPayeesCommand command, ISender mediator) =>
            {
                var importedCount = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { ImportedCount = importedCount, Message = $"Successfully imported {importedCount} payees" });
            })
            .WithName("ImportPayees")
            .WithSummary("Import payees from Excel file")
            .WithDescription("Imports payees from an Excel (.xlsx) file with validation, duplicate checking, and TIN validation")
            .Produces<object>()
            .ProducesValidationProblem()
            .RequirePermission(FshPermission.NameFor(FshActions.Import, FshResources.Accounting))
            .DisableAntiforgery();

        // Export endpoint
        group.MapPost("/export", async (ExportPayeesQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.File(response.Data, response.ContentType, response.FileName);
            })
            .WithName("ExportPayees")
            .WithSummary("Export payees to Excel file")
            .WithDescription("Exports payees to Excel (.xlsx) file with optional filtering by expense account, search criteria, TIN presence, and active status")
            .Produces<FileResult>()
            .ProducesValidationProblem()
            .RequirePermission(FshPermission.NameFor(FshActions.Export, FshResources.Accounting));
    }
}
