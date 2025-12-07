using Accounting.Infrastructure.Endpoints.Member.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Member;

/// <summary>
/// Endpoint configuration for Member module.
/// Provides comprehensive REST API endpoints for managing member.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class MemberEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Member endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/member").WithTags("member");

        group.MapMemberActivateEndpoint();
        group.MapMemberCreateEndpoint();
        group.MapMemberDeactivateEndpoint();
        group.MapMemberDeleteEndpoint();
        group.MapMemberGetEndpoint();
        group.MapMemberSearchEndpoint();
        group.MapMemberUpdateBalanceEndpoint();
        group.MapMemberUpdateEndpoint();
    }
}
