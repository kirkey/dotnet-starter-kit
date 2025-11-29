using FSH.Framework.Core.Storage.Commands;
using FSH.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Todo.Features.Import.v1;

/// <summary>
/// Endpoint for importing Todos from Excel format.
/// </summary>
public static class ImportTodosEndpoint
{
    internal static RouteHandlerBuilder MapImportTodosEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/import", async (
                ImportTodosCommand command,
                ISender mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .WithName(nameof(ImportTodosEndpoint))
            .WithSummary("Import todos from Excel file")
            .WithDescription(
                "Imports todos from Excel format. Returns the result of the import operation.")
            .Produces<ImportResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Import, FshResources.Todos))
            .MapToApiVersion(1);
    }
}
