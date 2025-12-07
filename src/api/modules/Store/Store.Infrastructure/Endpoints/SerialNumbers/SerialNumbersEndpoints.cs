using Store.Infrastructure.Endpoints.SerialNumbers.v1;
using Carter;

namespace Store.Infrastructure.Endpoints.SerialNumbers;

/// <summary>
/// Endpoint configuration for Serial Numbers module.
/// Provides REST API endpoints for managing serial numbers.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class SerialNumbersEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Serial Numbers endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and Search operations.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/serial-numbers").WithTags("serial-numbers");

        group.MapCreateSerialNumberEndpoint();
        group.MapUpdateSerialNumberEndpoint();
        group.MapDeleteSerialNumberEndpoint();
        group.MapGetSerialNumberEndpoint();
        group.MapSearchSerialNumbersEndpoint();
    }
}
