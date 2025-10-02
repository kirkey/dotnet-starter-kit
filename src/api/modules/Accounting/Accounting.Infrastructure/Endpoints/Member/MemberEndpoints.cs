namespace Accounting.Infrastructure.Endpoints.Member;

/// <summary>
/// Endpoint configuration for Member module.
/// </summary>
public static class MemberEndpoints
{
    /// <summary>
    /// Maps all Member endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapMemberEndpoints(this IEndpointRouteBuilder app)
    {
        var memberGroup = app.MapGroup("/members")
            .WithTags("Members")
            .WithDescription("Endpoints for managing member accounts");

        // Version 1 endpoints
        // memberGroup.MapMemberCreateEndpoint();
        // memberGroup.MapMemberUpdateEndpoint();
        // memberGroup.MapMemberDeleteEndpoint();
        // memberGroup.MapMemberGetEndpoint();
        // memberGroup.MapMemberSearchEndpoint();

        return app;
    }
}
