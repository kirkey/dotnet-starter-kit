using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Queries;
using FSH.Starter.WebApi.Todo.Domain;
using FSH.Starter.WebApi.Todo.Features.Specifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Todo.Features.Export.v1;

/// <summary>
/// Handler for exporting Todos to Excel format.
/// Uses the generic export infrastructure with custom filtering and mapping.
/// </summary>
public sealed class ExportTodosHandler(
    IDataExportService exportService,
    [FromKeyedServices("todo")] IReadRepository<TodoItem> repository,
    ILogger<ExportTodosHandler> logger)
    : IRequestHandler<ExportTodosQuery, ExportResponse>
{
    public async Task<ExportResponse> Handle(ExportTodosQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Starting export for Todos");

        try
        {
            // Build specification from filter
            var specification = request.Filter != null 
                ? new ExportTodosSpec(request.Filter) 
                : null;

            // Fetch data from repository
            var entities = specification != null
                ? await repository.ListAsync(specification, cancellationToken)
                : await repository.ListAsync(cancellationToken);

            logger.LogInformation("Found {Count} Todos to export", entities.Count);

            // Export to Excel
            var excelBytes = exportService.ExportToBytes(entities, request.SheetName);

            var fileName = $"Todos_Export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
            
            logger.LogInformation("Successfully exported {Count} Todos", entities.Count);

            return ExportResponse.Create(excelBytes, entities.Count, fileName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Export failed for Todos");
            throw new InvalidOperationException($"Export failed: {ex.Message}", ex);
        }
    }
}
