using Store.Infrastructure.Endpoints.Bins.v1;

namespace Store.Infrastructure.Endpoints.Bins;

/// <summary>
/// Carter module for Bins endpoints.
/// Routes all requests to separate versioned endpoint handlers.
/// </summary>
public class BinsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Bins endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/bins").WithTags("bins");

        // Call the individual endpoint handler methods from v1
        group.MapCreateBinEndpoint();
        group.MapGetBinEndpoint();
        group.MapUpdateBinEndpoint();
        group.MapDeleteBinEndpoint();
        group.MapSearchBinsEndpoint();
    }
}
