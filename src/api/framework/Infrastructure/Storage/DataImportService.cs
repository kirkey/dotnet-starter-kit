using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Framework.Infrastructure.Storage;

/// <summary>
/// Generic implementation for importing data from Excel files into strongly-typed objects.
/// Supports automatic property mapping, custom mappers, and validation.
/// </summary>
public sealed class DataImportService(ILogger<DataImportService> logger) : IDataImportService
{
    private readonly ILogger<DataImportService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Parses an Excel file into a list of strongly-typed objects using automatic property mapping.
    /// </summary>
    public async Task<IReadOnlyList<T>> ParseAsync<T>(
        FileUploadCommand file,
        string sheetName = "Sheet1",
        CancellationToken cancellationToken = default) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(file);

        _logger.LogInformation("Starting to parse file: {FileName} for type {TypeName}", file.Name, typeof(T).Name);

        try
        {
            var fileBytes = DecodeBase64File(file);
            using var memoryStream = new MemoryStream(fileBytes, writable: false);
            using var workbook = new XLWorkbook(memoryStream);

            var worksheet = GetWorksheet(workbook, sheetName);
            var results = ParseWorksheet<T>(worksheet, cancellationToken);

            _logger.LogInformation("Successfully parsed {Count} rows from file: {FileName}", results.Count, file.Name);

            return results.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse file: {FileName}", file.Name);
            throw new InvalidOperationException($"Failed to parse Excel file '{file.Name}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Parses an Excel file with custom column mapping function.
    /// </summary>
    public async Task<IReadOnlyList<T>> ParseWithMapperAsync<T>(
        FileUploadCommand file,
        Func<IDictionary<string, object?>, T> columnMapper,
        string sheetName = "Sheet1",
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(columnMapper);

        _logger.LogInformation("Starting to parse file with custom mapper: {FileName}", file.Name);

        try
        {
            var fileBytes = DecodeBase64File(file);
            using var memoryStream = new MemoryStream(fileBytes, writable: false);
            using var workbook = new XLWorkbook(memoryStream);

            var worksheet = GetWorksheet(workbook, sheetName);
            var results = ParseWorksheetWithMapper(worksheet, columnMapper, cancellationToken);

            _logger.LogInformation("Successfully parsed {Count} rows with custom mapper from file: {FileName}", results.Count, file.Name);

            return results.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse file with custom mapper: {FileName}", file.Name);
            throw new InvalidOperationException($"Failed to parse Excel file '{file.Name}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Validates an Excel file structure against expected columns.
    /// </summary>
    public async Task<ImportValidationResult> ValidateFileStructureAsync(
        FileUploadCommand file,
        IEnumerable<string> expectedColumns,
        string sheetName = "Sheet1",
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(expectedColumns);

        var expectedColumnsList = expectedColumns.ToList();
        if (expectedColumnsList.Count == 0)
        {
            return ImportValidationResult.Success();
        }

        try
        {
            var fileBytes = DecodeBase64File(file);
            using var memoryStream = new MemoryStream(fileBytes, writable: false);
            using var workbook = new XLWorkbook(memoryStream);

            var worksheet = GetWorksheet(workbook, sheetName);
            var headerRow = worksheet.FirstRowUsed();

            if (headerRow == null)
            {
                return ImportValidationResult.Failure(
                    new List<string> { "The Excel file is empty or has no header row." },
                    expectedColumnsList);
            }

            var actualColumns = headerRow.Cells()
                .Select(c => c.GetString().Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var missingColumns = expectedColumnsList
                .Where(expected => !actualColumns.Contains(expected))
                .ToList();

            if (missingColumns.Any())
            {
                var errors = new List<string>
                {
                    $"Missing required columns: {string.Join(", ", missingColumns)}"
                };

                return ImportValidationResult.Failure(errors, missingColumns);
            }

            return ImportValidationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate file structure: {FileName}", file.Name);
            return ImportValidationResult.Failure(
                new List<string> { $"File validation error: {ex.Message}" },
                expectedColumnsList);
        }
    }

    #region Private Helper Methods

    /// <summary>
    /// Decodes base64 file data from FileUploadCommand.
    /// </summary>
    private static byte[] DecodeBase64File(FileUploadCommand file)
    {
        if (string.IsNullOrWhiteSpace(file.Data))
        {
            throw new InvalidOperationException("File data is empty or invalid.");
        }

        try
        {
            return Convert.FromBase64String(file.Data);
        }
        catch (FormatException ex)
        {
            throw new InvalidOperationException($"File data is not valid base64: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Gets the worksheet by name from the workbook.
    /// </summary>
    private static IXLWorksheet GetWorksheet(XLWorkbook workbook, string sheetName)
    {
        var worksheet = workbook.Worksheets.FirstOrDefault(w => w.Name.Equals(sheetName, StringComparison.OrdinalIgnoreCase));
        
        if (worksheet == null)
        {
            throw new NotFoundException($"Worksheet '{sheetName}' not found in the Excel file.");
        }

        return worksheet;
    }

    /// <summary>
    /// Parses worksheet rows into strongly-typed objects using automatic property mapping.
    /// </summary>
    private List<T> ParseWorksheet<T>(IXLWorksheet worksheet, CancellationToken cancellationToken) where T : class, new()
    {
        var results = new List<T>();
        var headerRow = worksheet.FirstRowUsed();

        if (headerRow == null)
        {
            return results;
        }

        // Build column index map
        var columnMap = headerRow.Cells()
            .Select((cell, index) => new { ColumnName = cell.GetString().Trim(), Index = index + 1 })
            .Where(x => !string.IsNullOrWhiteSpace(x.ColumnName))
            .ToDictionary(x => x.ColumnName, x => x.Index, StringComparer.OrdinalIgnoreCase);

        var properties = typeof(T).GetProperties()
            .Where(p => p.CanWrite)
            .ToList();

        var dataRows = worksheet.RowsUsed().Skip(1);

        foreach (var row in dataRows)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var item = new T();

            foreach (var property in properties)
            {
                if (columnMap.TryGetValue(property.Name, out var columnIndex))
                {
                    try
                    {
                        var cellValue = row.Cell(columnIndex);
                        var value = ConvertCellValue(cellValue, property.PropertyType);
                        
                        if (value != null)
                        {
                            property.SetValue(item, value);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to set property {PropertyName} at row {RowNumber}", 
                            property.Name, row.RowNumber());
                    }
                }
            }

            results.Add(item);
        }

        return results;
    }

    /// <summary>
    /// Parses worksheet rows using a custom mapper function.
    /// </summary>
    private List<T> ParseWorksheetWithMapper<T>(
        IXLWorksheet worksheet,
        Func<IDictionary<string, object?>, T> columnMapper,
        CancellationToken cancellationToken) where T : class
    {
        var results = new List<T>();
        var headerRow = worksheet.FirstRowUsed();

        if (headerRow == null)
        {
            return results;
        }

        // Build column index map
        var columnMap = headerRow.Cells()
            .Select((cell, index) => new { ColumnName = cell.GetString().Trim(), Index = index + 1 })
            .Where(x => !string.IsNullOrWhiteSpace(x.ColumnName))
            .ToDictionary(x => x.ColumnName, x => x.Index, StringComparer.OrdinalIgnoreCase);

        var dataRows = worksheet.RowsUsed().Skip(1);

        foreach (var row in dataRows)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var rowData = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

            foreach (var column in columnMap)
            {
                var cellValue = row.Cell(column.Value);
                rowData[column.Key] = GetCellValueAsObject(cellValue);
            }

            var item = columnMapper(rowData);
            results.Add(item);
        }

        return results;
    }

    /// <summary>
    /// Converts an Excel cell value to the target property type.
    /// </summary>
    private static object? ConvertCellValue(IXLCell cell, Type targetType)
    {
        if (cell.IsEmpty() || cell.Value.IsBlank)
        {
            return null;
        }

        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        try
        {
            if (underlyingType == typeof(string))
            {
                return cell.GetString();
            }
            else if (underlyingType == typeof(int))
            {
                return Convert.ToInt32(cell.Value.GetNumber());
            }
            else if (underlyingType == typeof(long))
            {
                return Convert.ToInt64(cell.Value.GetNumber());
            }
            else if (underlyingType == typeof(decimal))
            {
                return Convert.ToDecimal(cell.Value.GetNumber());
            }
            else if (underlyingType == typeof(double))
            {
                return cell.Value.GetNumber();
            }
            else if (underlyingType == typeof(float))
            {
                return Convert.ToSingle(cell.Value.GetNumber());
            }
            else if (underlyingType == typeof(bool))
            {
                return cell.Value.GetBoolean();
            }
            else if (underlyingType == typeof(DateTime))
            {
                return cell.Value.GetDateTime();
            }
            else if (underlyingType == typeof(DateTimeOffset))
            {
                var dateTime = cell.Value.GetDateTime();
                return new DateTimeOffset(dateTime);
            }
            else if (underlyingType == typeof(DefaultIdType) || underlyingType == typeof(DefaultIdType))
            {
                var guidString = cell.GetString();
                return DefaultIdType.TryParse(guidString, out var guid) ? guid : null;
            }
            else if (underlyingType.IsEnum)
            {
                var stringValue = cell.GetString();
                return Enum.Parse(underlyingType, stringValue, ignoreCase: true);
            }
            else
            {
                return cell.GetString();
            }
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets cell value as generic object for custom mapper.
    /// </summary>
    private static object? GetCellValueAsObject(IXLCell cell)
    {
        if (cell.IsEmpty() || cell.Value.IsBlank)
        {
            return null;
        }

        return cell.Value.Type switch
        {
            XLDataType.Boolean => cell.Value.GetBoolean(),
            XLDataType.Number => cell.Value.GetNumber(),
            XLDataType.DateTime => cell.Value.GetDateTime(),
            XLDataType.TimeSpan => cell.Value.GetTimeSpan(),
            XLDataType.Text => cell.GetString(),
            _ => cell.GetString()
        };
    }

    #endregion
}

