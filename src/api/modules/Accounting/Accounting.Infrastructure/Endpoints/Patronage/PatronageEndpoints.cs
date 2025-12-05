using Accounting.Infrastructure.Endpoints.Patronage.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Patronage;

/// <summary>
/// Endpoint configuration for Patronage module.
/// Provides comprehensive REST API endpoints for managing patronage.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class PatronageEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all Patronage endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/patronage").WithTags("patronage");

        group.MapRetirePatronageEndpoint();
    }
}
