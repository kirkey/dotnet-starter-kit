using Accounting.Infrastructure.Endpoints.Payees.v1;

namespace Accounting.Infrastructure.Endpoints.Payees;

/// <summary>
/// Endpoint configuration for Payees module.
/// </summary>
public static class PayeesEndpoints
{
    /// <summary>
    /// Maps all Payees endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapPayeesEndpoints(this IEndpointRouteBuilder app)
    {
        var payeesGroup = app.MapGroup("/payees")
            .WithTags("Payees")
            .WithDescription("Endpoints for managing payees");

        // Version 1 endpoints
        payeesGroup.MapPayeeCreateEndpoint();
        payeesGroup.MapPayeeUpdateEndpoint();
        payeesGroup.MapPayeeDeleteEndpoint();
        payeesGroup.MapPayeeGetEndpoint();
        payeesGroup.MapPayeeSearchEndpoint();
        payeesGroup.MapPayeeImportEndpoint();
        payeesGroup.MapPayeeExportEndpoint();

        return app;
    }
}
