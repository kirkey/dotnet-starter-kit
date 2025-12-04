namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

/// <summary>
/// Endpoint configuration for Share Accounts.
/// </summary>
public static class ShareAccountEndpoints
{
    /// <summary>
    /// Maps all Share Account endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapShareAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var shareAccountsGroup = app.MapGroup("share-accounts").WithTags("share-accounts");

        shareAccountsGroup.MapGet("/", () => Results.Ok("Share Accounts endpoint - Coming soon"))
            .WithName("GetShareAccounts")
            .WithSummary("Gets all share accounts");

        return app;
    }
}
