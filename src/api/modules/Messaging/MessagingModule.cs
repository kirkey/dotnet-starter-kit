using Carter;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Starter.WebApi.Messaging.Features.Conversations.AddMember;
using FSH.Starter.WebApi.Messaging.Features.Conversations.AssignAdmin;
using FSH.Starter.WebApi.Messaging.Features.Conversations.Create;
using FSH.Starter.WebApi.Messaging.Features.Conversations.Delete;
using FSH.Starter.WebApi.Messaging.Features.Conversations.Get;
using FSH.Starter.WebApi.Messaging.Features.Conversations.GetList;
using FSH.Starter.WebApi.Messaging.Features.Conversations.RemoveMember;
using FSH.Starter.WebApi.Messaging.Features.Conversations.Update;
using FSH.Starter.WebApi.Messaging.Features.Conversations.MarkAsRead;
using FSH.Starter.WebApi.Messaging.Features.Messages.Create;
using FSH.Starter.WebApi.Messaging.Features.Messages.Delete;
using FSH.Starter.WebApi.Messaging.Features.Messages.Get;
using FSH.Starter.WebApi.Messaging.Features.Messages.GetList;
using FSH.Starter.WebApi.Messaging.Features.Messages.Update;
using FSH.Starter.WebApi.Messaging.Features.OnlineUsers;

namespace FSH.Starter.WebApi.Messaging;

/// <summary>
/// Module for configuring messaging services and endpoints.
/// Implements in-app messaging with conversations, messages, and file attachments.
/// </summary>
public static class MessagingModule
{
    /// <summary>
    /// Defines all messaging-related endpoints.
    /// </summary>
    public class Endpoints : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var conversationsGroup = app.MapGroup("conversations").WithTags("conversations");
            conversationsGroup.MapCreateConversationEndpoint();
            conversationsGroup.MapGetConversationEndpoint();
            conversationsGroup.MapGetConversationListEndpoint();
            conversationsGroup.MapUpdateConversationEndpoint();
            conversationsGroup.MapDeleteConversationEndpoint();
            conversationsGroup.MapAddMemberEndpoint();
            conversationsGroup.MapRemoveMemberEndpoint();
            conversationsGroup.MapAssignAdminEndpoint();
            conversationsGroup.MapMarkAsReadEndpoint();

            var messagesGroup = app.MapGroup("messages").WithTags("messages");
            messagesGroup.MapCreateMessageEndpoint();
            messagesGroup.MapGetMessageEndpoint();
            messagesGroup.MapGetMessageListEndpoint();
            messagesGroup.MapUpdateMessageEndpoint();

            var usersGroup = app.MapGroup("messaging").WithTags("messaging");
            usersGroup.MapGetOnlineUsersEndpoint();
            messagesGroup.MapDeleteMessageEndpoint();
        }
    }

    /// <summary>
    /// Registers messaging module services with the application builder.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application builder for chaining.</returns>
    public static WebApplicationBuilder RegisterMessagingServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Register DbContext
        builder.Services.BindDbContext<MessagingDbContext>();

        // Register database initializer
        builder.Services.AddScoped<IDbInitializer, MessagingDbInitializer>();

        // Register repositories for aggregate root entities only
        builder.Services.AddKeyedScoped<IRepository<Conversation>, MessagingRepository<Conversation>>("messaging");
        builder.Services.AddKeyedScoped<IReadRepository<Conversation>, MessagingRepository<Conversation>>("messaging");

        builder.Services.AddKeyedScoped<IRepository<Message>, MessagingRepository<Message>>("messaging");
        builder.Services.AddKeyedScoped<IReadRepository<Message>, MessagingRepository<Message>>("messaging");

        return builder;
    }

    /// <summary>
    /// Configures the messaging module for use in the application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application for chaining.</returns>
    public static WebApplication UseMessagingModule(this WebApplication app)
    {
        // Map SignalR hub
        app.MapHub<Hubs.MessagingHub>("/hubs/messaging");
        
        return app;
    }
}

