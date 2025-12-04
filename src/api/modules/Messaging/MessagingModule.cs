using FSH.Framework.Infrastructure.Persistence;

namespace FSH.Starter.WebApi.Messaging;

/// <summary>
/// Module for configuring messaging services and endpoints.
/// Implements in-app messaging with conversations, messages, and file attachments.
/// Note: All messaging endpoints are auto-discovered via ICarterModule implementations.
/// </summary>
public static class MessagingModule
{
    // Messaging endpoints are auto-discovered by Carter via ICarterModule implementations
    // See: ConversationsEndpoints, MessagesEndpoints, MessagingUtilityEndpoints

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
        // Map SignalR hubs
        app.MapHub<Framework.Infrastructure.SignalR.ConnectionHub>("/hubs/connection");
        app.MapHub<Hubs.MessagingHub>("/hubs/messaging");
        
        return app;
    }
}

