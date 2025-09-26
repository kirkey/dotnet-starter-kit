using System.Text.RegularExpressions;
using ClosedXML.Excel;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Framework.Infrastructure.Storage;

public class DataImport : IDataImport
{
   public async Task<IList<T>> ToListAsync<T>(FileUploadCommand request, FileType supportedFileType, string sheetName = "Sheet1")
    {
        // Handle both data URI format and raw base64 data
        string base64Data;
        
        // Check if the data is in data URI format (data:document/xlsx,base64data)
        var match = Regex.Match(request.Data,
            $"data:{supportedFileType.ToString().ToLower()}/(?<type>.+?),(?<data>.+)");
        
        if (match.Success)
        {
            // Extract base64 data from data URI format
            base64Data = match.Groups["data"].Value;
        }
        else
        {
            // Assume it's raw base64 data
            base64Data = request.Data;
        }
        
        // Validate that we have base64 data
        if (string.IsNullOrWhiteSpace(base64Data))
        {
            throw new InvalidOperationException("File data is empty or invalid.");
        }
        
        byte[] fileBytes;
        try
        {
            fileBytes = Convert.FromBase64String(base64Data);
        }
        catch (FormatException ex)
        {
            throw new InvalidOperationException($"File data is not valid base64: {ex.Message}", ex);
        }
        
        if (fileBytes.Length == 0)
        {
            throw new InvalidOperationException("File data cannot be empty.");
        }
        
        var streamData = new MemoryStream(fileBytes);

        List<T> list = [];
        Type typeOfObject = typeof(T);

        using IXLWorkbook workbook = new XLWorkbook(streamData);
        // Read the first Sheet from Excel file.
        var worksheet = workbook.Worksheets.FirstOrDefault(w => w.Name == sheetName)
                        ?? throw new NotFoundException($"Sheet with name {sheetName} does not exist!");


        var properties = typeOfObject.GetProperties();
        // header column texts
        var columns = worksheet.FirstRow().Cells().Select((v, i) => new { v.Value, Index = i + 1 });

        // indexing in closedxml starts with 1 not from 0
        // Skip first row which is used for column header texts
        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
        {
            T item = (T)Activator.CreateInstance(typeOfObject)!;

            foreach (var prop in properties)
            {
                try
                {
                    var propertyType = prop.PropertyType;
                    var col = columns.SingleOrDefault(c => c.Value.ToString() == prop.Name);
                    if (col == null) continue;

                    object? obj = GetObjectByDataType(propertyType, row.Cell(col.Index).Value);
                        
                    if(obj != null) prop.SetValue(item, obj);
                }
                catch
                {
                    // if any error
                    // return await Task.FromResult(new List<T>())
                }
            }

            list.Add(item);
        }

        return await Task.FromResult(list);
    }
    
    private static object? GetObjectByDataType(Type propertyType, XLCellValue cellValue)
    {
        if (cellValue.ToString() == "null" || cellValue.IsBlank)
        {
            return null;
        }

        object? val;
        if (propertyType.IsEnum)
        {
            val = Convert.ToInt32(cellValue.GetNumber());
            return Enum.ToObject(propertyType, val);
        }
        else if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
        {
            val = Guid.Parse(cellValue.ToString());
        }
        else if (propertyType == typeof(int) || propertyType == typeof(int?))
        {
            val = Convert.ToInt32(cellValue.GetNumber());
        }
        else if (propertyType == typeof(decimal))
        {
            val = Convert.ToDecimal(cellValue.GetNumber());
        }
        else if (propertyType == typeof(long))
        {
            val = Convert.ToInt64(cellValue.GetNumber());
        }
        else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
        {
            val = Convert.ToBoolean(cellValue.GetBoolean());
        }
        else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
        {
            val = Convert.ToDateTime(cellValue.GetDateTime());
        }
        else
        {
            val = cellValue.ToString();
        }

        return Convert.ChangeType(val, Nullable.GetUnderlyingType(propertyType) ?? propertyType);
    }
}
