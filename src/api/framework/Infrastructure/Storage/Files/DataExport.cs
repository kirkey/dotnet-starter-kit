using ClosedXML.Excel;
using ClosedXML.Report;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using FSH.Framework.Core.Storage;

namespace FSH.Framework.Infrastructure.Storage.Files;

/// <summary>
/// Provides helper methods for exporting arbitrary object collections to Excel (XLSX) either
/// as raw byte arrays, streams, or using a ClosedXML template (XLTemplate).
/// </summary>
public class DataExport : IDataExport
{
    /// <summary>
    /// Converts a non-null, non-empty list of objects to an in-memory Excel file represented as a byte array.
    /// The worksheet name defaults to "Sheet1".
    /// </summary>
    /// <typeparam name="T">The element type of the list.</typeparam>
    /// <param name="list">The list of items to export.</param>
    /// <returns>Byte array containing the XLSX file.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is null or empty.</exception>
    public byte[] ListToByteArray<T>(IList<T> list)
    {
        ValidateInputList(list, nameof(list));

        DataTable dataTable = BuildDataTable(list, typeof(T).Name);
        using XLWorkbook workbook = DataTableToIxlWorkbook("Sheet1", dataTable);

        using MemoryStream memoryStream = new();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Builds a <see cref="DataTable"/> from a list of objects of type <typeparamref name="T"/>.
    /// Each public instance property becomes a column.
    /// </summary>
    private static DataTable BuildDataTable<T>(IEnumerable<T> data, string? tableName = null)
    {
        ArgumentNullException.ThrowIfNull(data);
        var list = data as IList<T> ?? data.ToList();
        if (list.Count == 0)
        {
            throw new ArgumentNullException(nameof(data), "The collection must contain at least one element.");
        }

        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        if (properties.Length == 0)
        {
            throw new InvalidOperationException($"Type '{typeof(T).Name}' does not contain any public instance properties to export.");
        }

        DataTable dataTable = new(tableName ?? typeof(T).Name);

        foreach (PropertyInfo property in properties)
        {
            dataTable.Columns.Add(property.Name);
        }

        foreach (T item in list)
        {
            object?[] values = new object?[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(item, null) ?? DBNull.Value;
            }
            dataTable.Rows.Add(values);
        }

        return dataTable;
    }

    /// <summary>
    /// Creates an <see cref="XLWorkbook"/> from a populated <see cref="DataTable"/>.
    /// </summary>
    private static XLWorkbook DataTableToIxlWorkbook(string workbookName, DataTable dataTable)
    {
        if (string.IsNullOrWhiteSpace(workbookName))
        {
            throw new ArgumentNullException(nameof(workbookName));
        }
        if (dataTable is null || dataTable.Rows.Count == 0)
        {
            throw new ArgumentNullException(nameof(dataTable));
        }

        XLWorkbook workbook = new();
        workbook.Worksheets.Add(dataTable, workbookName);
        return workbook;
    }

    /// <summary>
    /// Writes a list of objects to an Excel workbook and returns a readable <see cref="Stream"/> positioned at 0.
    /// </summary>
    /// <typeparam name="T">Type of the elements to export.</typeparam>
    /// <param name="data">Collection of items.</param>
    /// <returns>A memory stream containing the XLSX file.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="data"/> is null or empty.</exception>
    public Stream WriteToStream<T>(IList<T> data)
    {
        ValidateInputList(data, nameof(data));

        // Use TypeDescriptor to preserve original property types when possible.
        var properties = TypeDescriptor.GetProperties(typeof(T));
        if (properties.Count == 0)
        {
            throw new InvalidOperationException($"Type '{typeof(T).Name}' does not contain any public properties to export.");
        }

        using var table = new DataTable(typeof(T).Name, "table");
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

        using var wb = new XLWorkbook();
        wb.Worksheets.Add(table);

        Stream stream = new MemoryStream();
        wb.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    /// <summary>
    /// Processes an Excel template file (XLTemplate) injecting <paramref name="data"/> variables and returns the generated workbook as a stream.
    /// Also saves the generated file to the specified <paramref name="outputFolder"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data model bound to the template.</typeparam>
    /// <param name="data">The data object to bind to the template.</param>
    /// <param name="templateFile">Full path (or relative path) to the template .xlsx file.</param>
    /// <param name="outputFolder">Directory where the processed file will be saved. Created if it does not exist.</param>
    /// <returns>Stream containing the processed Excel file.</returns>
    /// <exception cref="FileNotFoundException">If the template file does not exist.</exception>
    /// <exception cref="ArgumentException">If parameters are invalid.</exception>
    public Stream WriteToTemplate<T>(T data, string templateFile, string outputFolder)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        if (string.IsNullOrWhiteSpace(templateFile)) throw new ArgumentException("Template file path must be provided.", nameof(templateFile));
        if (string.IsNullOrWhiteSpace(outputFolder)) throw new ArgumentException("Output folder must be provided.", nameof(outputFolder));

        string templatePath = Path.GetFullPath(templateFile);
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException("Template file not found.", templatePath);
        }

        string resolvedOutputFolder = Path.GetFullPath(outputFolder);
        Directory.CreateDirectory(resolvedOutputFolder);

        var template = new XLTemplate(templatePath);
        template.AddVariable(data);
        template.Generate();

        string outputFileName = Path.GetFileName(templatePath);
        string outputPath = Path.Combine(resolvedOutputFolder, outputFileName);
        template.SaveAs(outputPath);

        Stream stream = new MemoryStream();
        template.Workbook.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    /// <summary>
    /// Validates that the provided list is not null and contains at least one element.
    /// </summary>
    private static void ValidateInputList<T>(IList<T> list, string paramName)
    {
        if (list is null || list.Count == 0)
        {
            throw new ArgumentNullException(paramName, "The collection must not be null or empty.");
        }
    }
}
