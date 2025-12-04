using Accounting.Application.ChartOfAccounts.Activate.v1;
using Accounting.Application.ChartOfAccounts.Create.v1;
using Accounting.Application.ChartOfAccounts.Deactivate.v1;
using Accounting.Application.ChartOfAccounts.Delete.v1;
using Accounting.Application.ChartOfAccounts.Export.v1;
using Accounting.Application.ChartOfAccounts.Get.v1;
using Accounting.Application.ChartOfAccounts.Import.v1;
using Accounting.Application.ChartOfAccounts.Responses;
using Accounting.Application.ChartOfAccounts.Search.v1;
using Accounting.Application.ChartOfAccounts.Update.v1;
using Accounting.Application.ChartOfAccounts.UpdateBalance.v1;
using Carter;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Storage.Commands;
using FSH.Framework.Core.Storage.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts;

/// <summary>
/// Endpoint configuration for Chart of Accounts module.
/// </summary>
public class ChartOfAccountsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Chart of Accounts endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/chart-of-accounts").WithTags("chart-of-accounts");

        group.MapPost("/", async (CreateChartOfAccountCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/chart-of-accounts/{response}", response);
            })
            .WithName("CreateChartOfAccount")
            .WithSummary("Create a chart of account")
            .WithDescription("Creates a new chart of account")
            .Produces<DefaultIdType>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetChartOfAccountRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetChartOfAccount")
            .WithSummary("Get chart of account by ID")
            .WithDescription("Retrieves a specific chart of account by its ID")
            .Produces<ChartOfAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateChartOfAccountCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateChartOfAccount")
            .WithSummary("Update a chart of account")
            .WithDescription("Updates an existing chart of account")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteChartOfAccountCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteChartOfAccount")
            .WithSummary("Delete a chart of account")
            .WithDescription("Deletes a chart of account")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapPost("/search", async (ISender mediator, [FromBody] SearchChartOfAccountRequest request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchChartOfAccounts")
            .WithSummary("Search chart of accounts")
            .WithDescription("Searches for chart of accounts with pagination and filtering")
            .Produces<PagedList<ChartOfAccountResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapPost("/{id}/activate", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new ActivateChartOfAccountCommand(id);
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("ActivateChartOfAccount")
            .WithSummary("Activate chart of account")
            .WithDescription("Activates a chart of account")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapPost("/{id}/deactivate", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeactivateChartOfAccountCommand(id);
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("DeactivateChartOfAccount")
            .WithSummary("Deactivate chart of account")
            .WithDescription("Deactivates a chart of account")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapPut("/{id}/balance", async (DefaultIdType id, UpdateChartOfAccountBalanceCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID in URL does not match ID in request body");

                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("UpdateChartOfAccountBalance")
            .WithSummary("Update chart of account balance")
            .WithDescription("Updates a chart of account balance")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapPost("/import", async (ImportChartOfAccountsCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("ImportChartOfAccounts")
            .WithSummary("Import chart of accounts from Excel file")
            .WithDescription("Imports chart of accounts from an Excel file with validation. Returns ImportResponse with successful/failed counts and detailed error messages.")
            .Produces<ImportResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Import, FshResources.Accounting))
            .MapToApiVersion(1);

        group.MapPost("/export", async (ExportChartOfAccountsQuery query, ISender mediator) =>
            {
                var result = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("ExportChartOfAccounts")
            .WithSummary("Export chart of accounts to Excel file")
            .WithDescription("Exports chart of accounts to Excel format with optional filtering. Returns an ExportResponse with file data.")
            .Produces<ExportResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Export, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
