namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Route group configuration for check endpoints.
/// </summary>
public static class CheckEndpoints
{
    public static IEndpointRouteBuilder MapCheckEndpointsV1(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/checks")
            .WithTags("Checks")
            .WithOpenApi();

        // Command endpoints
        group.MapCheckCreateEndpoint();
        group.MapCheckIssueEndpoint();
        group.MapCheckVoidEndpoint();
        group.MapCheckClearEndpoint();
        group.MapCheckStopPaymentEndpoint();
        group.MapCheckPrintEndpoint();

        // Query endpoints
        group.MapCheckGetEndpoint();
        group.MapCheckSearchEndpoint();

        return app;
    }
}


