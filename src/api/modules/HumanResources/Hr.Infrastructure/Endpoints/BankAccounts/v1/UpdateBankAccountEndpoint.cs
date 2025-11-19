using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BankAccounts.v1;

/// <summary>
/// Endpoint for updating a bank account.
/// </summary>
public static class UpdateBankAccountEndpoint
{
    internal static RouteHandlerBuilder MapUpdateBankAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateBankAccountCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateBankAccountEndpoint))
            .WithSummary("Updates a bank account")
            .WithDescription("Updates bank account details. Account numbers are encrypted at rest.")
            .Produces<UpdateBankAccountResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

