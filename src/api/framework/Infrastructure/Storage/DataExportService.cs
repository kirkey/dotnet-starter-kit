using ClosedXML.Report;

namespace FSH.Framework.Infrastructure.Storage;

/// <summary>
/// Generic implementation for exporting data collections to Excel files.
/// Supports automatic property mapping, custom column configuration, templates, and multi-sheet exports.
/// </summary>
public sealed class DataExportService(ILogger<DataExportService> logger) : IDataExportService
{
    private readonly ILogger<DataExportService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Exports a list of objects to Excel format as a byte array.
    /// </summary>
    public byte[] ExportToBytes<T>(IEnumerable<T> data, string sheetName = "Sheet1") where T : class
    {
        ArgumentNullException.ThrowIfNull(data);

        var dataList = data.ToList();
        ValidateDataNotEmpty(dataList);

        _logger.LogInformation("Exporting {Count} records of type {TypeName} to bytes", dataList.Count, typeof(T).Name);

        try
        {
            using var workbook = CreateWorkbookFromData(dataList, sheetName);
            using var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export data to bytes for type {TypeName}", typeof(T).Name);
            throw new InvalidOperationException($"Failed to export data to Excel: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Exports a list of objects to Excel format as a stream.
    /// </summary>
    public Stream ExportToStream<T>(IEnumerable<T> data, string sheetName = "Sheet1") where T : class
    {
        ArgumentNullException.ThrowIfNull(data);

        var dataList = data.ToList();
        ValidateDataNotEmpty(dataList);

        _logger.LogInformation("Exporting {Count} records of type {TypeName} to stream", dataList.Count, typeof(T).Name);

        try
        {
            using var workbook = CreateWorkbookFromData(dataList, sheetName);
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export data to stream for type {TypeName}", typeof(T).Name);
            throw new InvalidOperationException($"Failed to export data to Excel: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Exports data with custom column configuration.
    /// </summary>
    public Stream ExportWithConfigurationToStream<T>(
        IEnumerable<T> data,
        IEnumerable<ExportColumnConfiguration<T>> columnConfigurations,
        string sheetName = "Sheet1") where T : class
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(columnConfigurations);

        var dataList = data.ToList();
        var configList = columnConfigurations.OrderBy(c => c.Order).ToList();

        ValidateDataNotEmpty(dataList);

        if (configList.Count == 0)
        {
            throw new ArgumentException("Column configurations cannot be empty.", nameof(columnConfigurations));
        }

        _logger.LogInformation("Exporting {Count} records with {ColumnCount} custom columns", 
            dataList.Count, configList.Count);

        try
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Add headers
            for (int i = 0; i < configList.Count; i++)
            {
                var config = configList[i];
                var headerCell = worksheet.Cell(1, i + 1);
                headerCell.Value = config.HeaderName;
                headerCell.Style.Font.Bold = true;
                headerCell.Style.Fill.BackgroundColor = XLColor.LightGray;

                if (config.ColumnWidth.HasValue)
                {
                    worksheet.Column(i + 1).Width = config.ColumnWidth.Value;
                }
            }

            // Add data rows
            for (int rowIndex = 0; rowIndex < dataList.Count; rowIndex++)
            {
                var item = dataList[rowIndex];
                
                for (int colIndex = 0; colIndex < configList.Count; colIndex++)
                {
                    var config = configList[colIndex];
                    var cell = worksheet.Cell(rowIndex + 2, colIndex + 1);
                    var value = config.ValueSelector(item);

                    if (value != null)
                    {
                        if (!string.IsNullOrWhiteSpace(config.Format))
                        {
                            cell.Value = string.Format(config.Format, value);
                        }
                        else
                        {
                            cell.Value = XLCellValue.FromObject(value);
                        }
                    }
                }
            }

            // Auto-fit columns that don't have custom width
            for (int i = 0; i < configList.Count; i++)
            {
                if (!configList[i].ColumnWidth.HasValue)
                {
                    worksheet.Column(i + 1).AdjustToContents();
                }
            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export data with custom configuration");
            throw new InvalidOperationException($"Failed to export data with custom configuration: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Exports data using a template file.
    /// </summary>
    public Stream ExportUsingTemplate<T>(T data, string templateFilePath, string outputFolder) where T : class
    {
        ArgumentNullException.ThrowIfNull(data);

        if (string.IsNullOrWhiteSpace(templateFilePath))
        {
            throw new ArgumentException("Template file path must be provided.", nameof(templateFilePath));
        }

        if (string.IsNullOrWhiteSpace(outputFolder))
        {
            throw new ArgumentException("Output folder must be provided.", nameof(outputFolder));
        }

        _logger.LogInformation("Exporting data using template: {TemplatePath}", templateFilePath);

        try
        {
            string templatePath = Path.GetFullPath(templateFilePath);
            
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException("Template file not found.", templatePath);
            }

            string resolvedOutputFolder = Path.GetFullPath(outputFolder);
            Directory.CreateDirectory(resolvedOutputFolder);

            using var template = new XLTemplate(templatePath);
            template.AddVariable(data);
            template.Generate();

            string outputFileName = Path.GetFileName(templatePath);
            string outputPath = Path.Combine(resolvedOutputFolder, outputFileName);
            template.SaveAs(outputPath);

            Stream stream = new MemoryStream();
            template.Workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            
            _logger.LogInformation("Successfully exported data using template to: {OutputPath}", outputPath);
            
            return stream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export data using template: {TemplatePath}", templateFilePath);
            throw new InvalidOperationException($"Failed to export using template: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Exports multiple sheets in a single workbook.
    /// </summary>
    public Stream ExportMultipleSheetsToStream(IDictionary<string, IEnumerable<object>> sheets)
    {
        ArgumentNullException.ThrowIfNull(sheets);

        if (sheets.Count == 0)
        {
            throw new ArgumentException("Sheets dictionary cannot be empty.", nameof(sheets));
        }

        _logger.LogInformation("Exporting {SheetCount} sheets to a single workbook", sheets.Count);

        try
        {
            using var workbook = new XLWorkbook();

            foreach (var sheet in sheets)
            {
                var sheetName = ValidateSheetName(sheet.Key);
                var data = sheet.Value.ToList();

                if (data.Count == 0)
                {
                    _logger.LogWarning("Sheet '{SheetName}' has no data, skipping", sheetName);
                    continue;
                }

                using var dataTable = ConvertToDataTable(data, sheetName);
                workbook.Worksheets.Add(dataTable);
            }

            if (workbook.Worksheets.Count == 0)
            {
                throw new InvalidOperationException("No sheets with data were added to the workbook.");
            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            
            _logger.LogInformation("Successfully exported {SheetCount} sheets", workbook.Worksheets.Count);
            
            return stream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export multiple sheets");
            throw new InvalidOperationException($"Failed to export multiple sheets: {ex.Message}", ex);
        }
    }

    #region Private Helper Methods

    /// <summary>
    /// Creates an XLWorkbook from a list of objects using automatic property mapping.
    /// </summary>
    private static XLWorkbook CreateWorkbookFromData<T>(IList<T> data, string sheetName) where T : class
    {
        var properties = TypeDescriptor.GetProperties(typeof(T));
        
        if (properties.Count == 0)
        {
            throw new InvalidOperationException($"Type '{typeof(T).Name}' does not contain any public properties to export.");
        }

        using var table = new DataTable(sheetName);
        
        foreach (PropertyDescriptor prop in properties)
        {
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        }

        foreach (var item in data)
        {
            var row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
            {
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            }
            table.Rows.Add(row);
        }

        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(table);
        
        // Format headers
        var headerRow = worksheet.FirstRow();
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
        
        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        return workbook;
    }

    /// <summary>
    /// Converts a list of objects to a DataTable.
    /// </summary>
    private static DataTable ConvertToDataTable(List<object> data, string tableName)
    {
        if (data.Count == 0)
        {
            throw new ArgumentException("Data list cannot be empty.", nameof(data));
        }

        var firstItem = data[0];
        var properties = TypeDescriptor.GetProperties(firstItem);

        var table = new DataTable(tableName);

        foreach (PropertyDescriptor prop in properties)
        {
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        }

        foreach (var item in data)
        {
            var row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
            {
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            }
            table.Rows.Add(row);
        }

        return table;
    }

    /// <summary>
    /// Validates that the data list is not empty.
    /// </summary>
    private static void ValidateDataNotEmpty<T>(IList<T> data)
    {
        if (data.Count == 0)
        {
            throw new ArgumentException("The data collection must contain at least one element.", nameof(data));
        }
    }

    /// <summary>
    /// Validates and sanitizes sheet name for Excel compatibility.
    /// </summary>
    private static string ValidateSheetName(string sheetName)
    {
        if (string.IsNullOrWhiteSpace(sheetName))
        {
            return "Sheet1";
        }

        // Excel sheet name constraints: max 31 chars, no special chars
        var sanitized = sheetName.Trim();
        var invalidChars = new[] { '\\', '/', '*', '[', ']', ':', '?' };
        
        foreach (var c in invalidChars)
        {
            sanitized = sanitized.Replace(c.ToString(), "_");
        }

        if (sanitized.Length > 31)
        {
            sanitized = sanitized[..31];
        }

        return sanitized;
    }

    #endregion
}

