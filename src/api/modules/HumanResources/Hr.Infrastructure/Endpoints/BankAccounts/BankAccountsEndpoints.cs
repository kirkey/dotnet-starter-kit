using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Update.v1;
using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BankAccounts;

/// <summary>
/// Endpoint routes for managing bank accounts.
/// </summary>
public class BankAccountsEndpoints() : CarterModule("humanresources")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/bank-accounts")
            .WithTags("bank-accounts");

        group.MapPost("/", async (CreateBankAccountCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetBankAccount", new { id = response.Id }, response);
            })
            .WithName("CreateBankAccountEndpoint")
            .WithSummary("Creates a new bank account")
            .WithDescription("Creates a new bank account for an employee for direct deposit. Account numbers are encrypted at rest.")
            .Produces<CreateBankAccountResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetBankAccountRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetBankAccountEndpoint")
            .WithSummary("Gets a bank account by ID")
            .WithDescription("Retrieves detailed information about a specific bank account. Account numbers are masked for security.")
            .Produces<BankAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateBankAccountCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateBankAccountEndpoint")
            .WithSummary("Updates a bank account")
            .WithDescription("Updates bank account details. Account numbers are encrypted at rest.")
            .Produces<UpdateBankAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteBankAccountCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteBankAccountEndpoint")
            .WithSummary("Deletes a bank account")
            .WithDescription("Deletes a bank account. Cannot delete if it's the primary account.")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchBankAccountsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchBankAccountsEndpoint")
            .WithSummary("Searches bank accounts")
            .WithDescription("Searches and filters bank accounts by employee, bank name, account type with pagination support. Account numbers are masked for security.")
            .Produces<PagedList<BankAccountResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

