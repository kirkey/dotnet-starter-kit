using Store.Infrastructure.Endpoints.LotNumbers.v1;
using Carter;

namespace Store.Infrastructure.Endpoints.LotNumbers;

/// <summary>
/// Endpoint configuration for Lot Numbers module.
/// Provides REST API endpoints for managing lot/batch numbers.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class LotNumbersEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Lot Numbers endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and Search operations.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/lot-numbers").WithTags("lot-numbers");

        group.MapCreateLotNumberEndpoint();
        group.MapUpdateLotNumberEndpoint();
        group.MapDeleteLotNumberEndpoint();
        group.MapGetLotNumberEndpoint();
        group.MapSearchLotNumbersEndpoint();
    }
}
