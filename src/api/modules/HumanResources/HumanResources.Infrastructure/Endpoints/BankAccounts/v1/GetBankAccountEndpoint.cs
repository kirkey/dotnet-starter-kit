using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BankAccounts.v1;

/// <summary>
/// Endpoint for retrieving a specific bank account by ID.
/// </summary>
public static class GetBankAccountEndpoint
{
    internal static RouteHandlerBuilder MapGetBankAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetBankAccountRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetBankAccountEndpoint))
            .WithSummary("Gets a bank account by ID")
            .WithDescription("Retrieves detailed information about a specific bank account. Account numbers are masked for security.")
            .Produces<BankAccountResponse>()
            .RequirePermission("Permissions.BankAccounts.View")
            .MapToApiVersion(1);
    }
}

