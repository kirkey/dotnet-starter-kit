using Accounting.Infrastructure.Endpoints.SecurityDeposits.v1;

namespace Accounting.Infrastructure.Endpoints.SecurityDeposits;

/// <summary>
/// Endpoint configuration for Security Deposits module.
/// Provides comprehensive REST API endpoints for managing customer security deposits.
/// </summary>
public static class SecurityDepositsEndpoints
{
    /// <summary>
    /// Maps all Security Deposits endpoints to the route builder.
    /// Includes Create operation for security deposits.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <returns>The configured endpoint route builder.</returns>
    internal static IEndpointRouteBuilder MapSecurityDepositsEndpoints(this IEndpointRouteBuilder app)
    {
        var securityDepositsGroup = app.MapGroup("/security-deposits")
            .WithTags("Security-Deposits")
            .WithDescription("Endpoints for managing customer security deposits")
            .MapToApiVersion(1);

        // Version 1 endpoints
        securityDepositsGroup.MapSecurityDepositCreateEndpoint();

        return app;
    }
}

