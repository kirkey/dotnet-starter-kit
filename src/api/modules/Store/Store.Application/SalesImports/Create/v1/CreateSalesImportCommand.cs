namespace FSH.Starter.WebApi.Store.Application.SalesImports.Create.v1;

/// <summary>
/// Command to create and process a sales import from POS CSV data.
/// </summary>
public class CreateSalesImportCommand : IRequest<CreateSalesImportResponse>
{
    public string ImportNumber { get; set; } = default!;
    public DateTime ImportDate { get; set; }
    public DateTime SalesPeriodFrom { get; set; }
    public DateTime SalesPeriodTo { get; set; }
    public DefaultIdType WarehouseId { get; set; }
    public string FileName { get; set; } = default!;
    public string CsvData { get; set; } = default!;
    public string? Notes { get; set; }
    public bool AutoProcess { get; set; } = true;
}

/// <summary>
/// Response for sales import creation.
/// </summary>
public class CreateSalesImportResponse
{
    public DefaultIdType Id { get; set; }
    public string ImportNumber { get; set; } = default!;
    public string Status { get; set; } = default!;
    public int TotalRecords { get; set; }
    public int ProcessedRecords { get; set; }
    public int ErrorRecords { get; set; }

    public CreateSalesImportResponse(
        DefaultIdType id,
        string importNumber,
        string status,
        int totalRecords,
        int processedRecords,
        int errorRecords)
    {
        Id = id;
        ImportNumber = importNumber;
        Status = status;
        TotalRecords = totalRecords;
        ProcessedRecords = processedRecords;
        ErrorRecords = errorRecords;
    }
}

