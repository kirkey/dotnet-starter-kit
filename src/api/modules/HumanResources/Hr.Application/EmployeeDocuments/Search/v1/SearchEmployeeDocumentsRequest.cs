using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Search.v1;

/// <summary>
/// Request to search employee documents with filtering and pagination.
/// </summary>
public class SearchEmployeeDocumentsRequest : PaginationFilter, IRequest<PagedList<EmployeeDocumentResponse>>
{
    /// <summary>
    /// Gets or sets the search string to filter documents by title.
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary>
    /// Gets or sets the employee ID to filter documents for a specific employee.
    /// </summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the document type filter.
    /// </summary>
    public string? DocumentType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by expiry status.
    /// </summary>
    public bool? IsExpired { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show only expired documents.
    /// </summary>
    public bool? ExpiredOnly { get; set; }

    /// <summary>
    /// Gets or sets the start date for expiry date range filter.
    /// </summary>
    public DateTime? ExpiryDateStart { get; set; }

    /// <summary>
    /// Gets or sets the end date for expiry date range filter.
    /// </summary>
    public DateTime? ExpiryDateEnd { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by active status.
    /// </summary>
    public bool? IsActive { get; set; }
}
