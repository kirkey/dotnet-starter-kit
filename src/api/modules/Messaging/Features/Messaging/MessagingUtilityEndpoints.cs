using Carter;
using FSH.Starter.WebApi.Messaging.Features.OnlineUsers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Messaging.Features.Messaging;

/// <summary>
/// Endpoint configuration for Messaging module utilities.
/// </summary>
public class MessagingUtilityEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("messaging").WithTags("messaging");

        // Get online users
        group.MapGetOnlineUsersEndpoint();
    }
}
