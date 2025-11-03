using Accounting.Application.SecurityDeposits.Commands;

namespace Accounting.Infrastructure.Endpoints.SecurityDeposits.v1;

/// <summary>
/// Endpoint for creating a new security deposit.
/// </summary>
public static class SecurityDepositCreateEndpoint
{
    /// <summary>
    /// Maps the security deposit create endpoint.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <returns>The configured endpoint route builder.</returns>
    internal static RouteHandlerBuilder MapSecurityDepositCreateEndpoint(this IEndpointRouteBuilder app)
    {
        return app.MapPost("/", CreateSecurityDepositAsync)
            .WithName(nameof(SecurityDepositCreateEndpoint))
            .WithSummary("Create a new security deposit")
            .WithDescription("Creates a new security deposit for a member")
            .Produces<CreateSecurityDepositResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> CreateSecurityDepositAsync(
        CreateSecurityDepositCommand request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(request, cancellationToken).ConfigureAwait(false);
        return Results.Ok(response);
    }
}

