using Accounting.Infrastructure.Endpoints.Accruals.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Accruals;

/// <summary>
/// Endpoint configuration for Accruals module.
/// Provides comprehensive REST API endpoints for managing accruals.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class AccrualsEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all Accruals endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/accruals").WithTags("accruals");

        // CRUD operations
        group.MapCreateAccrualEndpoint();
        group.MapGetAccrualEndpoint();
        group.MapUpdateAccrualEndpoint();
        group.MapDeleteAccrualEndpoint();
        group.MapSearchAccrualsEndpoint();

        // Business operations
        group.MapApproveAccrualEndpoint();
        group.MapRejectAccrualEndpoint();
        group.MapReverseAccrualEndpoint();
    }
}
