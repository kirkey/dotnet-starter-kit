using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BankAccounts.v1;

/// <summary>
/// Endpoint for deleting a bank account.
/// </summary>
public static class DeleteBankAccountEndpoint
{
    internal static RouteHandlerBuilder MapDeleteBankAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new DeleteBankAccountCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteBankAccountEndpoint))
            .WithSummary("Deletes a bank account")
            .WithDescription("Deletes a bank account. Cannot delete if it's the primary account.")
            .Produces<DeleteBankAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

