using Accounting.Application.Banks.Get.v1;
using Accounting.Application.Banks.Search.v1;

namespace Accounting.Infrastructure.Endpoints.Banks.v1;

/// <summary>
/// Endpoint for searching banks with filtering and pagination.
/// Supports advanced filtering by bank code, name, routing number, and SWIFT code.
/// </summary>
public static class BankSearchEndpoint
{
    /// <summary>
    /// Maps the bank search endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapBankSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (BankSearchCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BankSearchEndpoint))
            .WithSummary("Search banks")
            .WithDescription("Searches banks with filtering by bank code, name, routing number, SWIFT code, and active status. Supports pagination and sorting.")
            .Produces<PagedList<BankResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

