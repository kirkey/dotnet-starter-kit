using Carter;
using FSH.Starter.WebApi.Messaging.Features.OnlineUsers;

namespace FSH.Starter.WebApi.Messaging.Features.Messaging;

/// <summary>
/// Endpoint configuration for Messaging module utilities.
/// </summary>
public class MessagingUtilityEndpoints() : CarterModule("messaging")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("messaging").WithTags("messaging");

        // Get online users
        group.MapGetOnlineUsersEndpoint();
    }
}
