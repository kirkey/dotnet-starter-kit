using Accounting.Application.Banks.Get.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Banks.v1;

/// <summary>
/// Endpoint for retrieving a bank by its unique identifier.
/// </summary>
public static class BankGetEndpoint
{
    /// <summary>
    /// Maps the bank get endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapBankGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new BankGetRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BankGetEndpoint))
            .WithSummary("Get a bank by ID")
            .WithDescription("Retrieves a bank by its unique identifier with all details.")
            .Produces<BankResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

