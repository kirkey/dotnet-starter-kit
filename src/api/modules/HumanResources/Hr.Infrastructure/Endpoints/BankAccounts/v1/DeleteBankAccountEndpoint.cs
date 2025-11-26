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
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteBankAccountCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(DeleteBankAccountEndpoint))
            .WithSummary("Deletes a bank account")
            .WithDescription("Deletes a bank account. Cannot delete if it's the primary account.")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

