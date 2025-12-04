using Carter;
using FSH.Starter.WebApi.Messaging.Features.Messages.Create;
using FSH.Starter.WebApi.Messaging.Features.Messages.Delete;
using FSH.Starter.WebApi.Messaging.Features.Messages.GetList;
using FSH.Starter.WebApi.Messaging.Features.Messages.Update;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.Messaging.Features.Messages;

/// <summary>
/// Endpoint configuration for Messages module.
/// </summary>
public class MessagesEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("messages").WithTags("messages");

        // Create message
        group.MapCreateMessageEndpoint();

        // Get message list
        group.MapGetMessageListEndpoint();

        // Update message
        group.MapUpdateMessageEndpoint();

        // Delete message
        group.MapDeleteMessageEndpoint();
    }
}
