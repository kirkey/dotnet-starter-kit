using Accounting.Infrastructure.Endpoints.RetainedEarnings.v1;

namespace Accounting.Infrastructure.Endpoints.RetainedEarnings;

public static class RetainedEarningsEndpoints
{
    internal static IEndpointRouteBuilder MapRetainedEarningsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/retained-earnings")
            .WithTags("Retained Earnings")
            .WithDescription("Endpoints for managing retained earnings")
            .MapToApiVersion(1);

        group.MapRetainedEarningsCreateEndpoint();
        group.MapRetainedEarningsGetEndpoint();
        group.MapRetainedEarningsSearchEndpoint();

        return app;
    }
}

