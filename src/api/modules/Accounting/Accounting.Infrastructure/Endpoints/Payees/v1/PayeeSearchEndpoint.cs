using Accounting.Application.Payees.Get.v1;
using Accounting.Application.Payees.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Payees.v1;

/// <summary>
/// Endpoint for searching payees with pagination and filtering capabilities.
/// Follows REST API conventions with comprehensive search functionality.
/// </summary>
public static class PayeeSearchEndpoint
{
    /// <summary>
    /// Maps the payee search endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapPayeeSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (PayeeSearchCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PayeeSearchEndpoint))
            .WithSummary("Search payees with pagination")
            .WithDescription("Searches payees with comprehensive filtering capabilities including keyword search, payee code, name, expense account code, and TIN filters with pagination support.")
            .Produces<PagedList<PayeeResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
