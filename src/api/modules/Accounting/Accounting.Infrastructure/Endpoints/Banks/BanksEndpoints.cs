using Accounting.Infrastructure.Endpoints.Banks.v1;

namespace Accounting.Infrastructure.Endpoints.Banks;

/// <summary>
/// Endpoint configuration for Banks module.
/// Provides comprehensive REST API endpoints for managing bank accounts and information.
/// </summary>
public static class BanksEndpoints
{
    /// <summary>
    /// Maps all Banks endpoints to the route builder.
    /// Includes Create, Read, Update, Delete, and Search operations for banks.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <returns>The configured endpoint route builder.</returns>
    internal static IEndpointRouteBuilder MapBanksEndpoints(this IEndpointRouteBuilder app)
    {
        var banksGroup = app.MapGroup("/banks")
            .WithTags("Banks")
            .WithDescription("Endpoints for managing banks in the accounting system")
            .MapToApiVersion(1);

        // Version 1 endpoints
        banksGroup.MapBankCreateEndpoint();
        banksGroup.MapBankUpdateEndpoint();
        banksGroup.MapBankDeleteEndpoint();
        banksGroup.MapBankGetEndpoint();
        banksGroup.MapBankSearchEndpoint();

        return app;
    }
}

