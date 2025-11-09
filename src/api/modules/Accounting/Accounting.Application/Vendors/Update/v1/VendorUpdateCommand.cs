namespace Accounting.Application.Vendors.Update.v1;

/// <summary>
/// Command to update an existing vendor.
/// </summary>
public record VendorUpdateCommand : IRequest<VendorUpdateResponse>
{
    /// <summary>
    /// The ID of the vendor to update.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// Vendor code.
    /// </summary>
    public string? VendorCode { get; init; }
    
    /// <summary>
    /// Vendor name.
    /// </summary>
    public string? Name { get; init; }
    
    /// <summary>
    /// Physical address.
    /// </summary>
    public string? Address { get; init; }
    
    /// <summary>
    /// Billing address.
    /// </summary>
    public string? BillingAddress { get; init; }
    
    /// <summary>
    /// Contact person name.
    /// </summary>
    public string? ContactPerson { get; init; }
    
    /// <summary>
    /// Email address.
    /// </summary>
    public string? Email { get; init; }
    
    /// <summary>
    /// Payment terms.
    /// </summary>
    public string? Terms { get; init; }
    
    /// <summary>
    /// Expense account code.
    /// </summary>
    public string? ExpenseAccountCode { get; init; }
    
    /// <summary>
    /// Expense account name.
    /// </summary>
    public string? ExpenseAccountName { get; init; }
    
    /// <summary>
    /// Tax identification number.
    /// </summary>
    public string? Tin { get; init; }
    
    /// <summary>
    /// Phone number.
    /// </summary>
    public string? Phone { get; init; }
    
    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; init; }
}
