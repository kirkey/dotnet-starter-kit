using FSH.Framework.Core.Specifications;
using FSH.Framework.Core.Storage.Queries;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace FSH.Framework.Infrastructure.Storage.Handlers;

/// <summary>
/// Base handler for generic export operations.
/// Provides common export logic that can be reused across all modules.
/// </summary>
/// <typeparam name="TEntity">The domain entity type.</typeparam>
/// <typeparam name="TExportDto">The export DTO type.</typeparam>
/// <typeparam name="TFilter">The filter type for querying data.</typeparam>
public abstract class ExportHandlerBase<TEntity, TExportDto, TFilter>(
    IDataExportService exportService,
    IReadRepository<TEntity> repository,
    ILogger logger)
    : IRequestHandler<ExportQuery<TFilter, ExportResponse>, ExportResponse>
    where TEntity : class
    where TExportDto : class
    where TFilter : class
{
    private readonly IDataExportService _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));
    private readonly IReadRepository<TEntity> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Handles the export query.
    /// </summary>
    public async Task<ExportResponse> Handle(ExportQuery<TFilter, ExportResponse> request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Starting export for {EntityType}", typeof(TEntity).Name);

        try
        {
            // Build specification from filter
            var specification = request.Filter != null 
                ? BuildSpecification(request.Filter) 
                : null;

            // Fetch data from repository
            var entities = specification != null
                ? await _repository.ListAsync(specification, cancellationToken)
                : await _repository.ListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} {EntityType} records to export", entities.Count, typeof(TEntity).Name);

            // Map entities to export DTOs
            var exportData = entities.Select(MapToExportDto).ToList();

            // Handle empty data case
            if (exportData.Count == 0)
            {
                exportData.Add(CreateEmptyDto());
            }

            // Export to Excel
            var excelBytes = _exportService.ExportToBytes(exportData, request.SheetName);

            var fileName = GetExportFileName();
            
            _logger.LogInformation("Successfully exported {Count} {EntityType} records", entities.Count, typeof(TEntity).Name);

            return ExportResponse.Create(excelBytes, entities.Count, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Export failed for {EntityType}", typeof(TEntity).Name);
            throw new InvalidOperationException($"Export failed: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Builds a specification from the filter.
    /// Override this to implement entity-specific filtering logic.
    /// </summary>
    protected abstract Specification<TEntity>? BuildSpecification(TFilter filter);

    /// <summary>
    /// Maps a domain entity to an export DTO.
    /// Override this to implement entity-specific mapping logic.
    /// </summary>
    protected abstract TExportDto MapToExportDto(TEntity entity);

    /// <summary>
    /// Creates an empty DTO for Excel structure when no data is found.
    /// Override this to provide a custom empty DTO.
    /// </summary>
    protected virtual TExportDto CreateEmptyDto()
    {
        return Activator.CreateInstance<TExportDto>();
    }

    /// <summary>
    /// Gets the export file name.
    /// Override this to provide a custom file name.
    /// </summary>
    protected virtual string GetExportFileName()
    {
        return $"{typeof(TEntity).Name}Export_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
    }
}

/// <summary>
/// Base handler for generic export operations with custom column configuration.
/// Provides common export logic with customizable column mapping.
/// </summary>
/// <typeparam name="TEntity">The domain entity type.</typeparam>
/// <typeparam name="TExportDto">The export DTO type.</typeparam>
/// <typeparam name="TFilter">The filter type for querying data.</typeparam>
public abstract class GenericExportWithConfigurationHandlerBase<TEntity, TExportDto, TFilter>(
    IDataExportService exportService,
    IReadRepository<TEntity> repository,
    ILogger logger)
    : IRequestHandler<ExportQuery<TFilter, ExportResponse>, ExportResponse>
    where TEntity : class
    where TExportDto : class
    where TFilter : class
{
    private readonly IDataExportService _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));
    private readonly IReadRepository<TEntity> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Handles the export query with custom column configuration.
    /// </summary>
    public async Task<ExportResponse> Handle(ExportQuery<TFilter, ExportResponse> request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Starting export with custom configuration for {EntityType}", typeof(TEntity).Name);

        try
        {
            // Build specification from filter
            var specification = request.Filter != null 
                ? BuildSpecification(request.Filter) 
                : null;

            // Fetch data from repository
            var entities = specification != null
                ? await _repository.ListAsync(specification, cancellationToken)
                : await _repository.ListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} {EntityType} records to export", entities.Count, typeof(TEntity).Name);

            // Map entities to export DTOs
            var exportData = entities.Select(MapToExportDto).ToList();

            // Get column configurations
            var columnConfigurations = GetColumnConfigurations();

            // Export to Excel with custom configuration
            var stream = _exportService.ExportWithConfigurationToStream(exportData, columnConfigurations, request.SheetName);

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, cancellationToken);
            var excelBytes = memoryStream.ToArray();

            var fileName = GetExportFileName();
            
            _logger.LogInformation("Successfully exported {Count} {EntityType} records with custom configuration", 
                entities.Count, typeof(TEntity).Name);

            return ExportResponse.Create(excelBytes, entities.Count, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Export failed for {EntityType}", typeof(TEntity).Name);
            throw new InvalidOperationException($"Export failed: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Builds a specification from the filter.
    /// Override this to implement entity-specific filtering logic.
    /// </summary>
    protected abstract Specification<TEntity>? BuildSpecification(TFilter filter);

    /// <summary>
    /// Maps a domain entity to an export DTO.
    /// Override this to implement entity-specific mapping logic.
    /// </summary>
    protected abstract TExportDto MapToExportDto(TEntity entity);

    /// <summary>
    /// Gets the column configurations for the export.
    /// Override this to define custom column mappings, formats, and widths.
    /// </summary>
    protected abstract IEnumerable<ExportColumnConfiguration<TExportDto>> GetColumnConfigurations();

    /// <summary>
    /// Gets the export file name.
    /// Override this to provide a custom file name.
    /// </summary>
    protected virtual string GetExportFileName()
    {
        return $"{typeof(TEntity).Name}Export_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
    }
}

