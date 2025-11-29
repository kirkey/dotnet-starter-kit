using FSH.Framework.Core.Storage.Queries;
using MediatR;

namespace FSH.Starter.WebApi.Todo.Features.Export.v1;

/// <summary>
/// Query for exporting Todos to Excel format.
/// </summary>
public sealed record ExportTodosQuery : IRequest<ExportResponse>
{
    /// <summary>
    /// Filter criteria for the todos to export.
    /// </summary>
    public TodoExportFilter? Filter { get; init; }

    /// <summary>
    /// The worksheet name. Defaults to "Todos".
    /// </summary>
    public string SheetName { get; init; } = "Todos";
}

/// <summary>
/// Filter DTO for exporting todos with various criteria.
/// </summary>
public sealed class TodoExportFilter
{
    /// <summary>
    /// Search term for name, description, or notes.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Filter by creation date (after this date).
    /// </summary>
    public DateTime? CreatedAfter { get; set; }

    /// <summary>
    /// Filter by creation date (before this date).
    /// </summary>
    public DateTime? CreatedBefore { get; set; }
}
