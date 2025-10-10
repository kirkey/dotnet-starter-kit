# Generic Import/Export Infrastructure - Todo Module Example

This document demonstrates how to use the new generic import/export infrastructure with a real-world example using the Todo module.

## Example Implementation

### 1. Import Implementation

#### Import Command
```csharp
// Todo/Features/TodoItems/Import/ImportTodoItemsCommand.cs
using FSH.Framework.Core.Storage.Commands;
using FSH.Framework.Core.Storage.File.Features;

namespace Todo.Features.TodoItems.Import;

/// <summary>
/// Command for importing Todo Items from an Excel file.
/// </summary>
public sealed record ImportTodoItemsCommand : GenericImportCommand<ImportResponse>
{
    /// <summary>
    /// Creates a new import command.
    /// </summary>
    public ImportTodoItemsCommand(FileUploadCommand file, string sheetName = "Sheet1")
    {
        File = file;
        SheetName = sheetName;
        ValidateStructure = true;
    }
}
```

#### Import Handler
```csharp
// Todo/Features/TodoItems/Import/ImportTodoItemsHandler.cs
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Commands;
using FSH.Framework.Infrastructure.Storage.Handlers;
using Todo.Domain;

namespace Todo.Features.TodoItems.Import;

/// <summary>
/// Handler for importing Todo Items from Excel files.
/// Uses the generic import infrastructure with custom validation and mapping.
/// </summary>
public sealed class ImportTodoItemsHandler : GenericImportHandlerBase<TodoItem, TodoItemImportRow>
{
    private readonly IReadRepository<TodoItem> _readRepository;

    public ImportTodoItemsHandler(
        IDataImportService importService,
        IRepository<TodoItem> repository,
        IReadRepository<TodoItem> readRepository,
        ILogger<ImportTodoItemsHandler> logger)
        : base(importService, repository, logger)
    {
        _readRepository = readRepository;
    }

    /// <summary>
    /// Validates a single import row with strict business rules.
    /// </summary>
    protected override async Task<IReadOnlyList<string>> ValidateRowAsync(
        TodoItemImportRow row, 
        int rowIndex, 
        CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        // Validate required fields
        if (string.IsNullOrWhiteSpace(row.Title))
        {
            errors.Add($"Row {rowIndex}: Title is required");
        }
        else if (row.Title.Length > 200)
        {
            errors.Add($"Row {rowIndex}: Title cannot exceed 200 characters");
        }

        // Validate description length
        if (!string.IsNullOrWhiteSpace(row.Description) && row.Description.Length > 2000)
        {
            errors.Add($"Row {rowIndex}: Description cannot exceed 2000 characters");
        }

        // Validate priority
        if (!string.IsNullOrWhiteSpace(row.Priority))
        {
            var validPriorities = new[] { "Low", "Medium", "High" };
            if (!validPriorities.Contains(row.Priority, StringComparer.OrdinalIgnoreCase))
            {
                errors.Add($"Row {rowIndex}: Priority must be Low, Medium, or High");
            }
        }

        // Validate due date
        if (row.DueDate.HasValue && row.DueDate.Value < DateTime.UtcNow.Date)
        {
            errors.Add($"Row {rowIndex}: Due date cannot be in the past");
        }

        // Check for duplicates by title
        if (!string.IsNullOrWhiteSpace(row.Title))
        {
            var exists = await _readRepository.AnyAsync(
                new TodoItemByTitleSpec(row.Title.Trim()), 
                cancellationToken);
                
            if (exists)
            {
                errors.Add($"Row {rowIndex}: Todo item with title '{row.Title}' already exists");
            }
        }

        return errors;
    }

    /// <summary>
    /// Maps an import row to a domain entity.
    /// </summary>
    protected override async Task<TodoItem> MapToEntityAsync(
        TodoItemImportRow row, 
        CancellationToken cancellationToken)
    {
        var todoItem = TodoItem.Create(
            row.Title!.Trim(),
            row.Description?.Trim());

        if (!string.IsNullOrWhiteSpace(row.Priority))
        {
            todoItem.SetPriority(row.Priority.Trim());
        }

        if (row.DueDate.HasValue)
        {
            todoItem.SetDueDate(row.DueDate.Value);
        }

        if (row.IsCompleted == true)
        {
            todoItem.MarkAsCompleted();
        }

        if (!string.IsNullOrWhiteSpace(row.Tags))
        {
            var tags = row.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToList();
            
            foreach (var tag in tags)
            {
                todoItem.AddTag(tag);
            }
        }

        if (!string.IsNullOrWhiteSpace(row.Category))
        {
            todoItem.SetCategory(row.Category.Trim());
        }

        return await Task.FromResult(todoItem);
    }
}
```

#### Import Endpoint
```csharp
// Todo/Features/TodoItems/Import/ImportTodoItemsEndpoint.cs
namespace Todo.Features.TodoItems.Import;

/// <summary>
/// Endpoint for importing Todo Items from Excel files.
/// </summary>
public sealed class ImportTodoItemsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/todo-items/import", async (
            [FromBody] ImportTodoItemsCommand command,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            
            if (result.IsSuccess)
            {
                return Results.Ok(new 
                { 
                    Message = $"Successfully imported {result.ImportedCount} todo items",
                    Data = result 
                });
            }
            
            return Results.BadRequest(new 
            { 
                Message = "Import failed or partially completed",
                Data = result,
                Errors = result.Errors 
            });
        })
        .WithName("ImportTodoItems")
        .WithTags("TodoItems")
        .RequireAuthorization()
        .Produces<ImportResponse>(200)
        .Produces<ImportResponse>(400)
        .WithSummary("Import todo items from Excel file")
        .WithDescription("Imports todo items from an Excel file with validation. Returns count of successful and failed imports.");
    }
}
```

### 2. Export Implementation

#### Export Query
```csharp
// Todo/Features/TodoItems/Export/ExportTodoItemsQuery.cs
using FSH.Framework.Core.Storage.Queries;

namespace Todo.Features.TodoItems.Export;

/// <summary>
/// Query for exporting Todo Items to Excel format.
/// </summary>
public sealed record ExportTodoItemsQuery : GenericExportQuery<TodoItemFilterDto, ExportResponse>
{
    /// <summary>
    /// Creates a new export query with optional filters.
    /// </summary>
    public ExportTodoItemsQuery(TodoItemFilterDto? filter = null, string sheetName = "TodoItems")
    {
        Filter = filter;
        SheetName = sheetName;
    }
}

/// <summary>
/// Filter DTO for exporting todo items.
/// </summary>
public sealed class TodoItemFilterDto
{
    public bool? IsCompleted { get; set; }
    public string? Priority { get; set; }
    public string? Category { get; set; }
    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
    public string? SearchTerm { get; set; }
}
```

#### Export Handler
```csharp
// Todo/Features/TodoItems/Export/ExportTodoItemsHandler.cs
using FSH.Framework.Core.Specifications;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Queries;
using FSH.Framework.Infrastructure.Storage.Handlers;
using Todo.Domain;

namespace Todo.Features.TodoItems.Export;

/// <summary>
/// Handler for exporting Todo Items to Excel format.
/// Uses the generic export infrastructure with custom filtering and mapping.
/// </summary>
public sealed class ExportTodoItemsHandler : GenericExportHandlerBase<TodoItem, TodoItemExportDto, TodoItemFilterDto>
{
    public ExportTodoItemsHandler(
        IDataExportService exportService,
        IReadRepository<TodoItem> repository,
        ILogger<ExportTodoItemsHandler> logger)
        : base(exportService, repository, logger)
    {
    }

    /// <summary>
    /// Builds a specification from the filter criteria.
    /// </summary>
    protected override Specification<TodoItem>? BuildSpecification(TodoItemFilterDto filter)
    {
        return new ExportTodoItemsSpec(filter);
    }

    /// <summary>
    /// Maps a domain entity to an export DTO.
    /// </summary>
    protected override TodoItemExportDto MapToExportDto(TodoItem entity)
    {
        return new TodoItemExportDto
        {
            Title = entity.Title,
            Description = entity.Description ?? string.Empty,
            Priority = entity.Priority ?? "Medium",
            DueDate = entity.DueDate,
            IsCompleted = entity.IsCompleted,
            CompletedDate = entity.CompletedDate,
            Tags = string.Join(", ", entity.Tags ?? Array.Empty<string>()),
            Category = entity.Category ?? string.Empty,
            CreatedDate = entity.CreatedOn,
            CreatedBy = entity.CreatedBy?.ToString() ?? "System"
        };
    }

    /// <summary>
    /// Gets a custom export file name with timestamp.
    /// </summary>
    protected override string GetExportFileName()
    {
        return $"TodoItems_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
    }
}
```

#### Export Specification
```csharp
// Todo/Features/TodoItems/Export/ExportTodoItemsSpec.cs
using FSH.Framework.Core.Specifications;
using Todo.Domain;

namespace Todo.Features.TodoItems.Export;

/// <summary>
/// Specification for filtering todo items during export.
/// </summary>
public sealed class ExportTodoItemsSpec : Specification<TodoItem>
{
    public ExportTodoItemsSpec(TodoItemFilterDto filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        if (filter.IsCompleted.HasValue)
        {
            Query.Where(t => t.IsCompleted == filter.IsCompleted.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Priority))
        {
            Query.Where(t => t.Priority == filter.Priority);
        }

        if (!string.IsNullOrWhiteSpace(filter.Category))
        {
            Query.Where(t => t.Category == filter.Category);
        }

        if (filter.DueDateFrom.HasValue)
        {
            Query.Where(t => t.DueDate >= filter.DueDateFrom.Value);
        }

        if (filter.DueDateTo.HasValue)
        {
            Query.Where(t => t.DueDate <= filter.DueDateTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            Query.Where(t => t.Title.Contains(filter.SearchTerm) || 
                           (t.Description != null && t.Description.Contains(filter.SearchTerm)));
        }

        Query.OrderBy(t => t.DueDate)
             .ThenBy(t => t.Title);
    }
}
```

#### Export Endpoint
```csharp
// Todo/Features/TodoItems/Export/ExportTodoItemsEndpoint.cs
namespace Todo.Features.TodoItems.Export;

/// <summary>
/// Endpoint for exporting Todo Items to Excel format.
/// </summary>
public sealed class ExportTodoItemsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/todo-items/export", async (
            [FromBody] ExportTodoItemsQuery query,
            ISender mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(query, cancellationToken);
            
            return Results.File(
                result.Data,
                result.ContentType,
                result.FileName);
        })
        .WithName("ExportTodoItems")
        .WithTags("TodoItems")
        .RequireAuthorization()
        .Produces<FileResult>(200)
        .WithSummary("Export todo items to Excel file")
        .WithDescription("Exports todo items to Excel format with optional filtering by completion status, priority, category, and date range.");
    }
}
```

## Excel File Structure

### Import File Format

Create an Excel file with the following columns:

| Title | Description | Priority | DueDate | IsCompleted | Tags | Category |
|-------|-------------|----------|---------|-------------|------|----------|
| Complete project proposal | Draft the Q1 project proposal | High | 2025-10-15 | FALSE | work,urgent | Work |
| Review documentation | Review and update API docs | Medium | 2025-10-20 | FALSE | documentation | Development |
| Team meeting | Weekly team sync | Low | 2025-10-12 | TRUE | meeting | Management |

**Column Definitions:**
- **Title** (Required): The todo item title (max 200 chars)
- **Description** (Optional): Detailed description (max 2000 chars)
- **Priority** (Optional): Low, Medium, or High
- **DueDate** (Optional): Future date in Excel date format
- **IsCompleted** (Optional): TRUE or FALSE
- **Tags** (Optional): Comma-separated tags
- **Category** (Optional): Item category

### Export File Format

The exported Excel file will contain:

| Title | Description | Priority | DueDate | IsCompleted | CompletedDate | Tags | Category | CreatedDate | CreatedBy |
|-------|-------------|----------|---------|-------------|---------------|------|----------|-------------|-----------|
| Complete project proposal | Draft the Q1 project proposal | High | 2025-10-15 | FALSE | | work, urgent | Work | 2025-10-10 | admin@company.com |

## API Usage Examples

### Import Request

```bash
POST /api/v1/todo-items/import
Content-Type: application/json

{
  "file": {
    "name": "todo-items.xlsx",
    "data": "UEsDBBQABgAIAAAAIQBi...", // base64 encoded Excel file
    "extension": ".xlsx",
    "size": 15420
  },
  "sheetName": "Sheet1",
  "validateStructure": true
}
```

### Import Response (Success)

```json
{
  "message": "Successfully imported 25 todo items",
  "data": {
    "importedCount": 25,
    "failedCount": 0,
    "totalCount": 25,
    "errors": [],
    "isSuccess": true
  }
}
```

### Import Response (Partial Success)

```json
{
  "message": "Import failed or partially completed",
  "data": {
    "importedCount": 20,
    "failedCount": 5,
    "totalCount": 25,
    "errors": [
      "Row 3: Title is required",
      "Row 7: Priority must be Low, Medium, or High",
      "Row 12: Todo item with title 'Review code' already exists",
      "Row 15: Due date cannot be in the past",
      "Row 21: Description cannot exceed 2000 characters"
    ],
    "isSuccess": false
  }
}
```

### Export Request

```bash
POST /api/v1/todo-items/export
Content-Type: application/json

{
  "filter": {
    "isCompleted": false,
    "priority": "High",
    "dueDateFrom": "2025-10-01",
    "dueDateTo": "2025-10-31"
  },
  "sheetName": "TodoItems"
}
```

### Export Response

Returns an Excel file download with the name `TodoItems_20251010_143052.xlsx`

## Benefits of This Approach

1. **Minimal Code**: Handlers are concise, focusing only on business logic
2. **Reusable**: Same infrastructure works for all modules
3. **Type-Safe**: Strong typing prevents runtime errors
4. **Validated**: Built-in validation at multiple levels
5. **Flexible**: Easy to customize mapping and validation
6. **Documented**: Full XML documentation for IntelliSense
7. **Testable**: Easy to unit test with mocked dependencies
8. **Maintainable**: Changes to infrastructure benefit all modules

## Testing

```csharp
public class ImportTodoItemsHandlerTests
{
    [Fact]
    public async Task Should_Import_Valid_TodoItems()
    {
        // Arrange
        var importService = new Mock<IDataImportService>();
        var repository = new Mock<IRepository<TodoItem>>();
        var readRepository = new Mock<IReadRepository<TodoItem>>();
        var logger = new Mock<ILogger<ImportTodoItemsHandler>>();

        var handler = new ImportTodoItemsHandler(
            importService.Object,
            repository.Object,
            readRepository.Object,
            logger.Object);

        var rows = new List<TodoItemImportRow>
        {
            new() { Title = "Test Item", Priority = "High" }
        };

        importService
            .Setup(x => x.ParseAsync<TodoItemImportRow>(It.IsAny<FileUploadCommand>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(rows);

        var command = new ImportTodoItemsCommand(new FileUploadCommand 
        { 
            Name = "test.xlsx", 
            Data = "test" 
        });

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.ImportedCount);
    }
}
```

## Next Steps

1. Copy the DTO files to your module
2. Implement the handlers following the examples
3. Create the endpoints
4. Test with sample Excel files
5. Document the expected file format for users

