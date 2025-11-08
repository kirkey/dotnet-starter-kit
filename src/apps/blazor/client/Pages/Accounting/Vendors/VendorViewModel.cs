using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.Vendors;

/// <summary>
/// View model for creating and editing vendors.
/// </summary>
public class VendorViewModel
{
    /// <summary>
    /// Vendor identifier (null for new vendors).
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Unique vendor code.
    /// </summary>
    [Required(ErrorMessage = "Vendor code is required")]
    [StringLength(50, ErrorMessage = "Vendor code cannot exceed 50 characters")]
    public string VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// Vendor name.
    /// </summary>
    [Required(ErrorMessage = "Vendor name is required")]
    [StringLength(200, ErrorMessage = "Vendor name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Physical address.
    /// </summary>
    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; set; }

    /// <summary>
    /// Billing address (if different from physical address).
    /// </summary>
    [StringLength(500, ErrorMessage = "Billing address cannot exceed 500 characters")]
    public string? BillingAddress { get; set; }

    /// <summary>
    /// Contact person name.
    /// </summary>
    [StringLength(100, ErrorMessage = "Contact person cannot exceed 100 characters")]
    public string? ContactPerson { get; set; }

    /// <summary>
    /// Contact email address.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string? Email { get; set; }

    /// <summary>
    /// Payment terms (e.g., "Net 30", "Due on Receipt").
    /// </summary>
    [StringLength(50, ErrorMessage = "Terms cannot exceed 50 characters")]
    public string? Terms { get; set; }

    /// <summary>
    /// Default expense account code for this vendor.
    /// </summary>
    [StringLength(50, ErrorMessage = "Expense account code cannot exceed 50 characters")]
    public string? ExpenseAccountCode { get; set; }

    /// <summary>
    /// Default expense account name (for display).
    /// </summary>
    public string? ExpenseAccountName { get; set; }

    /// <summary>
    /// Tax Identification Number.
    /// </summary>
    [StringLength(50, ErrorMessage = "TIN cannot exceed 50 characters")]
    public string? Tin { get; set; }

    /// <summary>
    /// Phone number.
    /// </summary>
    [Phone(ErrorMessage = "Invalid phone number")]
    [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
    public string? Phone { get; set; }

    /// <summary>
    /// Description or notes about the vendor.
    /// </summary>
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters")]
    public string? Notes { get; set; }
}

