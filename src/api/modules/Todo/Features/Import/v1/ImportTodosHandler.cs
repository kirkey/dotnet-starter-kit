using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Commands;
using FSH.Starter.WebApi.Todo.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Todo.Features.Import.v1;

/// <summary>
/// Handler for importing Todos from Excel files.
/// Implements validation and mapping rules for TodoItems.
/// </summary>
public sealed class ImportTodosHandler(
    IDataImportService importService,
    [FromKeyedServices("todo")] IRepository<TodoItem> repository,
    [FromKeyedServices("todo")] IReadRepository<TodoItem> readRepository,
    ILogger<ImportTodosHandler> logger)
    : IRequestHandler<ImportTodosCommand, ImportResponse>
{
    public async Task<ImportResponse> Handle(ImportTodosCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.File);

        logger.LogInformation("Starting import for Todos from file: {FileName}", request.File.Name);

        try
        {
            // Validate file structure if required
            if (request.ValidateStructure)
            {
                var expectedColumns = GetExpectedColumns();
                var validationResult = await importService.ValidateFileStructureAsync(
                    request.File, expectedColumns, request.SheetName, cancellationToken);

                if (!validationResult.IsValid)
                {
                    logger.LogWarning("File structure validation failed: {Errors}", 
                        string.Join(", ", validationResult.Errors));
                    return ImportResponse.Failure(validationResult.Errors);
                }
            }

            // Parse the file
            var rows = await importService.ParseAsync<TodoImportRow>(
                request.File, request.SheetName, cancellationToken);

            if (rows.Count == 0)
            {
                logger.LogInformation("No rows found in the import file");
                return ImportResponse.Success(0);
            }

            // Process rows
            var result = await ProcessRowsAsync(rows, cancellationToken);

            logger.LogInformation("Import completed: {Imported} successful, {Failed} failed out of {Total} total",
                result.ImportedCount, result.FailedCount, result.TotalCount);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Import failed for Todos");
            return ImportResponse.Failure(new List<string> { $"Import failed: {ex.Message}" });
        }
    }

    private async Task<ImportResponse> ProcessRowsAsync(IReadOnlyList<TodoImportRow> rows, CancellationToken cancellationToken)
    {
        var errors = new List<string>();
        var validEntities = new List<TodoItem>();

        logger.LogInformation("Starting validation for {Count} rows", rows.Count);

        // Load existing todos to check for duplicates
        var existingTodos = await readRepository.ListAsync(cancellationToken);
        var existingNames = new HashSet<string>(existingTodos.Select(t => t.Name.ToUpperInvariant()), StringComparer.OrdinalIgnoreCase);

        // Track names from the import file to detect duplicates within the file
        var importNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Validate all rows
        for (int i = 0; i < rows.Count; i++)
        {
            var rowIndex = i + 1;
            var row = rows[i];

            try
            {
                // Validate the row
                var validationErrors = ValidateRow(row, rowIndex, existingNames, importNames);

                if (validationErrors.Any())
                {
                    errors.AddRange(validationErrors);
                    continue;
                }

                // Map to entity
                var entity = MapToEntity(row);
                validEntities.Add(entity);

                // Track names from this import batch
                importNames.Add(row.Name!.Trim().ToUpperInvariant());
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to process row {RowIndex}", rowIndex);
                errors.Add($"Row {rowIndex}: {ex.Message}");
            }
        }

        // Bulk insert valid entities
        var successfullyInserted = 0;
        if (validEntities.Count > 0)
        {
            logger.LogInformation("Inserting {Count} valid todos", validEntities.Count);

            try
            {
                await repository.AddRangeAsync(validEntities.ToArray(), cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);
                successfullyInserted = validEntities.Count;
                logger.LogInformation("Successfully inserted {Count} todos", successfullyInserted);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to insert todos");
                errors.Add($"Failed to insert todos: {ex.Message}");
            }
        }

        var failedCount = rows.Count - successfullyInserted;

        logger.LogInformation("Import summary: {Successful} successful, {Failed} failed, {Total} total", 
            successfullyInserted, failedCount, rows.Count);

        return failedCount > 0
            ? ImportResponse.PartialSuccess(successfullyInserted, failedCount, errors)
            : ImportResponse.Success(successfullyInserted);
    }

    private static IEnumerable<string> GetExpectedColumns()
    {
        return new[]
        {
            nameof(TodoImportRow.Name),
            nameof(TodoImportRow.Description),
            nameof(TodoImportRow.Notes)
        };
    }

    private static IReadOnlyList<string> ValidateRow(
        TodoImportRow row,
        int rowIndex,
        HashSet<string> existingNames,
        HashSet<string> importNames)
    {
        var errors = new List<string>();

        // Validate required fields
        if (string.IsNullOrWhiteSpace(row.Name))
        {
            errors.Add($"Row {rowIndex}: Name is required");
        }
        else if (row.Name.Trim().Length < TodoItem.NameMinLength)
        {
            errors.Add($"Row {rowIndex}: Name must be at least {TodoItem.NameMinLength} character");
        }
        else if (row.Name.Length > TodoItem.NameMaxLength)
        {
            errors.Add($"Row {rowIndex}: Name cannot exceed {TodoItem.NameMaxLength} characters");
        }
        else
        {
            var nameUpper = row.Name.Trim().ToUpperInvariant();
            
            // Check for duplicate name in existing todos
            if (existingNames.Contains(nameUpper))
            {
                errors.Add($"Row {rowIndex}: Todo with name '{row.Name}' already exists");
            }
            // Check for duplicate name within the import file
            else if (importNames.Contains(nameUpper))
            {
                errors.Add($"Row {rowIndex}: Duplicate name '{row.Name}' found within import file");
            }
        }

        // Validate optional fields
        if (!string.IsNullOrWhiteSpace(row.Description) && row.Description.Length > TodoItem.DescriptionMaxLength)
        {
            errors.Add($"Row {rowIndex}: Description cannot exceed {TodoItem.DescriptionMaxLength} characters");
        }

        if (!string.IsNullOrWhiteSpace(row.Notes) && row.Notes.Length > TodoItem.NotesMaxLength)
        {
            errors.Add($"Row {rowIndex}: Notes cannot exceed {TodoItem.NotesMaxLength} characters");
        }

        return errors;
    }

    private static TodoItem MapToEntity(TodoImportRow row)
    {
        return TodoItem.Create(
            name: row.Name!.Trim(),
            description: row.Description?.Trim(),
            notes: row.Notes?.Trim());
    }
}
