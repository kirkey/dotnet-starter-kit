using Accounting.Infrastructure.Endpoints.Banks.v1;

namespace Accounting.Infrastructure.Endpoints.Banks;

/// <summary>
/// Endpoint configuration for Banks module.
/// </summary>
public static class BanksEndpoints
{
    /// <summary>
    /// Maps all Banks endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapBanksEndpoints(this IEndpointRouteBuilder app)
    {
        var banksGroup = app.MapGroup("/banks")
            .WithTags("Banks")
            .WithDescription("Endpoints for managing banks");

        // Version 1 endpoints
        banksGroup.MapBankCreateEndpoint();
        banksGroup.MapBankUpdateEndpoint();
        banksGroup.MapBankDeleteEndpoint();
        banksGroup.MapBankGetEndpoint();
        banksGroup.MapBankSearchEndpoint();

        return app;
    }
}

