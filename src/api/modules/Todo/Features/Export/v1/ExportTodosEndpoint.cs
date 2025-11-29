using FSH.Framework.Core.Storage.Queries;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Todo.Features.Export.v1;

/// <summary>
/// Endpoint for exporting Todos to Excel format.
/// </summary>
public static class ExportTodosEndpoint
{
    internal static RouteHandlerBuilder MapExportTodosEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/export", async (
                ExportTodosQuery query,
                ISender mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(query, cancellationToken);
                return Results.Ok(result);
            })
            .WithName(nameof(ExportTodosEndpoint))
            .WithSummary("Export todos to Excel file")
            .WithDescription(
                "Exports todos to Excel format with optional filtering. Returns an ExportResponse with file data.")
            .Produces<ExportResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Export, FshResources.Todos))
            .MapToApiVersion(1);
    }
}
