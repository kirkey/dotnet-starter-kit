using Accounting.Infrastructure.Endpoints.Member.v1;

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
            .WithDescription("Endpoints for managing member accounts")
            .MapToApiVersion(1);

        // CRUD operations
        memberGroup.MapMemberCreateEndpoint();
        memberGroup.MapMemberGetEndpoint();
        memberGroup.MapMemberUpdateEndpoint();
        memberGroup.MapMemberDeleteEndpoint();
        memberGroup.MapMemberSearchEndpoint();

        // Workflow operations
        memberGroup.MapMemberActivateEndpoint();
        memberGroup.MapMemberDeactivateEndpoint();
        memberGroup.MapMemberUpdateBalanceEndpoint();

        return app;
    }
}
