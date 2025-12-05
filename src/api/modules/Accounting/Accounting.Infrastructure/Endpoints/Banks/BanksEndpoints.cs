using Accounting.Infrastructure.Endpoints.Banks.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Banks;

/// <summary>
/// Endpoint configuration for Banks module.
/// Provides comprehensive REST API endpoints for managing bank accounts and information.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class BanksEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all Banks endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and Search operations.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/banks").WithTags("banks");

        group.MapCreateBankEndpoint();
        group.MapGetBankEndpoint();
        group.MapUpdateBankEndpoint();
        group.MapDeleteBankEndpoint();
        group.MapSearchBankEndpoint();
    }
}

