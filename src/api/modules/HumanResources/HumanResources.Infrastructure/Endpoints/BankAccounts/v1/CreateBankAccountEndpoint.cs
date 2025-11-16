using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BankAccounts.v1;

/// <summary>
/// Endpoint for creating a new bank account.
/// </summary>
public static class CreateBankAccountEndpoint
{
    internal static RouteHandlerBuilder MapCreateBankAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBankAccountCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetBankAccountEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateBankAccountEndpoint))
            .WithSummary("Creates a new bank account")
            .WithDescription("Creates a new bank account for an employee for direct deposit. Account numbers are encrypted at rest.")
            .Produces<CreateBankAccountResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}

