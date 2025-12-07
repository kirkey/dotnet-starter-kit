using Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings;

/// <summary>
/// Endpoint configuration for RetainedEarnings module.
/// Provides comprehensive REST API endpoints for managing retained-earnings.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class RetainedEarningsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all RetainedEarnings endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/retained-earnings").WithTags("retained-earning");

        group.MapRetainedEarningsCloseEndpoint();
        group.MapRetainedEarningsCreateEndpoint();
        group.MapRetainedEarningsGetEndpoint();
        group.MapRetainedEarningsRecordDistributionEndpoint();
        group.MapRetainedEarningsReopenEndpoint();
        group.MapRetainedEarningsSearchEndpoint();
        group.MapRetainedEarningsUpdateNetIncomeEndpoint();
    }
}
