using Accounting.Infrastructure.Endpoints.WriteOffs.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.WriteOffs;

/// <summary>
/// Endpoint configuration for WriteOffs module.
/// Provides comprehensive REST API endpoints for managing write-offs.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class WriteOffsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all WriteOffs endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/write-offs").WithTags("write-off");

        group.MapWriteOffApproveEndpoint();
        group.MapWriteOffCreateEndpoint();
        group.MapWriteOffGetEndpoint();
        group.MapWriteOffPostEndpoint();
        group.MapWriteOffRecordRecoveryEndpoint();
        group.MapWriteOffRejectEndpoint();
        group.MapWriteOffReverseEndpoint();
        group.MapWriteOffSearchEndpoint();
        group.MapWriteOffUpdateEndpoint();
    }
}
