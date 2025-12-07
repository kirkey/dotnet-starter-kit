using Accounting.Infrastructure.Endpoints.SecurityDeposits.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.SecurityDeposits;

/// <summary>
/// Endpoint configuration for SecurityDeposits module.
/// Provides comprehensive REST API endpoints for managing security-deposits.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class SecurityDepositsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all SecurityDeposits endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/security-deposits").WithTags("security-deposit");

        group.MapSecurityDepositCreateEndpoint();
    }
}
