using Accounting.Infrastructure.Endpoints.Payees.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Payees;

/// <summary>
/// Endpoint configuration for Payees module.
/// Provides comprehensive REST API endpoints for managing payees.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class PayeesEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Payees endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/payees").WithTags("payee");

        group.MapPayeeCreateEndpoint();
        group.MapPayeeDeleteEndpoint();
        group.MapPayeeExportEndpoint();
        group.MapPayeeGetEndpoint();
        group.MapPayeeImportEndpoint();
        group.MapPayeeSearchEndpoint();
        group.MapPayeeUpdateEndpoint();
    }
}
