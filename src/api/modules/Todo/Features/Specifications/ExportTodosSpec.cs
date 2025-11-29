using Ardalis.Specification;
using FSH.Starter.WebApi.Todo.Domain;
using FSH.Starter.WebApi.Todo.Features.Export.v1;

namespace FSH.Starter.WebApi.Todo.Features.Specifications;

/// <summary>
/// Specification for filtering todos during export.
/// Applies various filter criteria to build a filtered query.
/// </summary>
public sealed class ExportTodosSpec : Specification<TodoItem>
{
    public ExportTodosSpec(TodoExportFilter? filter)
    {
        // Apply filters if provided
        if (filter == null)
        {
            // Order by name for consistent export
            Query.OrderBy(t => t.Name);
            return;
        }

        // Search term filter (name, description, or notes)
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.Trim();
            Query.Where(t => t.Name.Contains(searchTerm) ||
                           (t.Description != null && t.Description.Contains(searchTerm)) ||
                           (t.Notes != null && t.Notes.Contains(searchTerm)));
        }

        // Created after filter
        if (filter.CreatedAfter.HasValue)
        {
            Query.Where(t => t.CreatedOn >= filter.CreatedAfter.Value);
        }

        // Created before filter
        if (filter.CreatedBefore.HasValue)
        {
            Query.Where(t => t.CreatedOn <= filter.CreatedBefore.Value);
        }
        
        // Order by name for consistent export
        Query.OrderBy(t => t.Name);
    }
}
