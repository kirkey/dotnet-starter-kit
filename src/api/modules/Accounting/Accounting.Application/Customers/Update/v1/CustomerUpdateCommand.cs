namespace Accounting.Application.Customers.Update.v1;

/// <summary>
/// Command to update an existing customer.
/// </summary>
public record CustomerUpdateCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// The ID of the customer to update.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// Customer name.
    /// </summary>
    public string? CustomerName { get; init; }
    
    /// <summary>
    /// Billing address.
    /// </summary>
    public string? BillingAddress { get; init; }
    
    /// <summary>
    /// Shipping address.
    /// </summary>
    public string? ShippingAddress { get; init; }
    
    /// <summary>
    /// Email address.
    /// </summary>
    public string? Email { get; init; }
    
    /// <summary>
    /// Phone number.
    /// </summary>
    public string? Phone { get; init; }
    
    /// <summary>
    /// Contact name.
    /// </summary>
    public string? ContactName { get; init; }
    
    /// <summary>
    /// Contact email.
    /// </summary>
    public string? ContactEmail { get; init; }
    
    /// <summary>
    /// Contact phone.
    /// </summary>
    public string? ContactPhone { get; init; }
    
    /// <summary>
    /// Payment terms.
    /// </summary>
    public string? PaymentTerms { get; init; }
    
    /// <summary>
    /// Tax exempt status.
    /// </summary>
    public bool? TaxExempt { get; init; }
    
    /// <summary>
    /// Tax identification number.
    /// </summary>
    public string? TaxId { get; init; }
    
    /// <summary>
    /// Discount percentage.
    /// </summary>
    public decimal? DiscountPercentage { get; init; }
    
    /// <summary>
    /// Sales representative.
    /// </summary>
    public string? SalesRepresentative { get; init; }
    
    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; init; }
}
