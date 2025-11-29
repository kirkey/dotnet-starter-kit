using Carter;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Starter.WebApi.Todo.Domain;
using FSH.Starter.WebApi.Todo.Features.Create.v1;
using FSH.Starter.WebApi.Todo.Features.Delete.v1;
using FSH.Starter.WebApi.Todo.Features.Get.v1;
using FSH.Starter.WebApi.Todo.Features.GetList.v1;
using FSH.Starter.WebApi.Todo.Features.Update.v1;
using FSH.Starter.WebApi.Todo.Features.Export.v1;
using FSH.Starter.WebApi.Todo.Features.Import.v1;
using FSH.Starter.WebApi.Todo.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Todo;

/// <summary>
/// Todo module configuration and endpoint registration.
/// Handles all todo item CRUD operations and service registration.
/// </summary>
public static class TodoModule
{
    /// <summary>
    /// Endpoint routes for the Todo module.
    /// Maps all todo item endpoints with proper grouping and documentation.
    /// </summary>
    public class Endpoints : CarterModule
    {
        /// <summary>
        /// Adds all todo routes to the application.
        /// Organizes endpoints by operation type with proper tagging for OpenAPI.
        /// </summary>
        /// <param name="app">The endpoint route builder.</param>
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var todoGroup = app.MapGroup("todos").WithTags("todos");
            todoGroup.MapTodoItemCreateEndpoint();
            todoGroup.MapGetTodoEndpoint();
            todoGroup.MapGetTodoListEndpoint();
            todoGroup.MapTodoItemUpdateEndpoint();
            todoGroup.MapTodoItemDeletionEndpoint();
            todoGroup.MapExportTodosEndpoint();
            todoGroup.MapImportTodosEndpoint();
        }
    }

    /// <summary>
    /// Registers all todo services in the dependency injection container.
    /// Configures DbContext, repositories, and database initializers.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application builder for chaining.</returns>
    public static WebApplicationBuilder RegisterTodoServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<TodoDbContext>();
        builder.Services.AddScoped<IDbInitializer, TodoDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<TodoItem>, TodoRepository<TodoItem>>("todo");
        builder.Services.AddKeyedScoped<IReadRepository<TodoItem>, TodoRepository<TodoItem>>("todo");
        return builder;
    }

    /// <summary>
    /// Applies the todo module to the web application.
    /// Currently a no-op placeholder for future middleware or configuration.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>The web application for chaining.</returns>
    public static WebApplication UseTodoModule(this WebApplication app)
    {
        return app;
    }
}
