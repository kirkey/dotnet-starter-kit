using FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Search.v1;

/// <summary>
/// Command to search serial numbers with pagination and filtering.
/// </summary>
public class SearchSerialNumbersCommand : PaginationFilter, IRequest<PagedList<SerialNumberResponse>>
{
    /// <summary>
    /// Serial number value to filter by.
    /// </summary>
    public string? SerialNumberValue { get; set; }
    
    /// <summary>
    /// Item ID to filter by.
    /// </summary>
    public DefaultIdType? ItemId { get; set; }
    
    /// <summary>
    /// Warehouse ID to filter by.
    /// </summary>
    public DefaultIdType? WarehouseId { get; set; }
    
    /// <summary>
    /// Warehouse location ID to filter by.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; set; }
    
    /// <summary>
    /// Bin ID to filter by.
    /// </summary>
    public DefaultIdType? BinId { get; set; }
    
    /// <summary>
    /// Lot number ID to filter by.
    /// </summary>
    public DefaultIdType? LotNumberId { get; set; }
    
    /// <summary>
    /// Status to filter by (e.g., Available, Allocated, Sold, Returned, Defective).
    /// </summary>
    public string? Status { get; set; }
    
    /// <summary>
    /// Filter by receipt date from (inclusive).
    /// </summary>
    public DateTime? ReceiptDateFrom { get; set; }
    
    /// <summary>
    /// Filter by receipt date to (inclusive).
    /// </summary>
    public DateTime? ReceiptDateTo { get; set; }
    
    /// <summary>
    /// Filter by whether the serial number has a warranty.
    /// </summary>
    public bool? HasWarranty { get; set; }
    
    /// <summary>
    /// Filter by whether the warranty is expired.
    /// </summary>
    public bool? IsWarrantyExpired { get; set; }
    
    /// <summary>
    /// External reference to filter by.
    /// </summary>
    public string? ExternalReference { get; set; }
}

