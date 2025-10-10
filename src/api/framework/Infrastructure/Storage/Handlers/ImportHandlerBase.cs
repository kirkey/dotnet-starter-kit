using FSH.Framework.Core.Storage.Commands;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace FSH.Framework.Infrastructure.Storage.Handlers;

/// <summary>
/// Base handler for generic import operations.
/// Provides common import logic that can be reused across all modules.
/// </summary>
/// <typeparam name="TEntity">The domain entity type.</typeparam>
/// <typeparam name="TImportRow">The import row DTO type.</typeparam>
public abstract class ImportHandlerBase<TEntity, TImportRow>(
    IDataImportService importService,
    IRepository<TEntity> repository,
    ILogger logger)
    : IRequestHandler<ImportCommand<ImportResponse>, ImportResponse>
    where TEntity : class
    where TImportRow : class, new()
{
    private readonly IDataImportService _importService = importService ?? throw new ArgumentNullException(nameof(importService));
    private readonly IRepository<TEntity> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Handles the import command.
    /// </summary>
    public async Task<ImportResponse> Handle(ImportCommand<ImportResponse> request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.File);

        _logger.LogInformation("Starting import for {EntityType} from file: {FileName}", 
            typeof(TEntity).Name, request.File.Name);

        try
        {
            // Validate file structure if required
            if (request.ValidateStructure)
            {
                var expectedColumns = GetExpectedColumns();
                var validationResult = await _importService.ValidateFileStructureAsync(
                    request.File, expectedColumns, request.SheetName, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("File structure validation failed: {Errors}", 
                        string.Join(", ", validationResult.Errors));
                    return ImportResponse.Failure(validationResult.Errors);
                }
            }

            // Parse the file
            var rows = await _importService.ParseAsync<TImportRow>(
                request.File, request.SheetName, cancellationToken);

            if (rows.Count == 0)
            {
                _logger.LogInformation("No rows found in the import file");
                return ImportResponse.Success(0);
            }

            // Process rows
            var result = await ProcessRowsAsync(rows, cancellationToken);

            _logger.LogInformation("Import completed: {Imported} successful, {Failed} failed out of {Total} total",
                result.ImportedCount, result.FailedCount, result.TotalCount);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Import failed for {EntityType}", typeof(TEntity).Name);
            return ImportResponse.Failure(new List<string> { $"Import failed: {ex.Message}" });
        }
    }

    /// <summary>
    /// Processes the imported rows and returns the result.
    /// Derived classes must implement this to define entity-specific processing logic.
    /// </summary>
    protected async Task<ImportResponse> ProcessRowsAsync(IReadOnlyList<TImportRow> rows, CancellationToken cancellationToken)
    {
        var errors = new List<string>();
        var importedCount = 0;

        for (int i = 0; i < rows.Count; i++)
        {
            var rowIndex = i + 1;
            var row = rows[i];

            try
            {
                // Validate the row
                var validationErrors = await ValidateRowAsync(row, rowIndex, cancellationToken);
                if (validationErrors.Any())
                {
                    errors.AddRange(validationErrors);
                    continue;
                }

                // Map to entity
                var entity = await MapToEntityAsync(row, cancellationToken);

                // Save entity
                await _repository.AddAsync(entity, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);

                importedCount++;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to import row {RowIndex}", rowIndex);
                errors.Add($"Row {rowIndex}: {ex.Message}");
            }
        }

        var failedCount = rows.Count - importedCount;
        return failedCount > 0
            ? ImportResponse.PartialSuccess(importedCount, failedCount, errors)
            : ImportResponse.Success(importedCount);
    }

    /// <summary>
    /// Gets the expected column names for file structure validation.
    /// Override this to provide custom column validation.
    /// </summary>
    protected virtual IEnumerable<string> GetExpectedColumns()
    {
        return typeof(TImportRow).GetProperties().Select(p => p.Name);
    }

    /// <summary>
    /// Validates a single import row.
    /// Override this to implement entity-specific validation logic.
    /// </summary>
    /// <param name="row">The import row to validate.</param>
    /// <param name="rowIndex">The 1-based row index for error messages.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of validation error messages.</returns>
    protected abstract Task<IReadOnlyList<string>> ValidateRowAsync(TImportRow row, int rowIndex, CancellationToken cancellationToken);

    /// <summary>
    /// Maps an import row to a domain entity.
    /// Override this to implement entity-specific mapping logic.
    /// </summary>
    /// <param name="row">The import row to map.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The mapped domain entity.</returns>
    protected abstract Task<TEntity> MapToEntityAsync(TImportRow row, CancellationToken cancellationToken);
}

