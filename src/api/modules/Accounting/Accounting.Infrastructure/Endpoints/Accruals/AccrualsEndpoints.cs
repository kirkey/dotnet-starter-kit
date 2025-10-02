using Accounting.Infrastructure.Endpoints.Accruals.v1;

namespace Accounting.Infrastructure.Endpoints.Accruals;

/// <summary>
/// Endpoint configuration for Accruals module.
/// </summary>
public static class AccrualsEndpoints
{
    /// <summary>
    /// Maps all Accruals endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapAccrualsEndpoints(this IEndpointRouteBuilder app)
    {
        var accrualsGroup = app.MapGroup("/accruals")
            .WithTags("Accruals")
            .WithDescription("Endpoints for managing accrual entries");

        // Version 1 endpoints
        accrualsGroup.MapAccrualCreateEndpoint();
        accrualsGroup.MapAccrualUpdateEndpoint();
        accrualsGroup.MapAccrualDeleteEndpoint();
        accrualsGroup.MapAccrualGetEndpoint();
        accrualsGroup.MapAccrualReverseEndpoint();
        accrualsGroup.MapAccrualSearchEndpoint();

        return app;
    }
}
