using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Update.v1;

/// <summary>
/// Command to update an existing supplier.
/// </summary>
public record UpdateSupplierCommand : IRequest<UpdateSupplierResponse>
{
    /// <summary>
    /// Gets or sets the supplier identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the supplier name.
    /// </summary>
    [DefaultValue("ABC Supplies Inc.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the supplier description.
    /// </summary>
    [DefaultValue("Reliable supplier of industrial goods")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the supplier code.
    /// </summary>
    [DefaultValue("SUP-001")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the main contact person at the supplier.
    /// </summary>
    [DefaultValue("John Smith")]
    public string ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact email.
    /// </summary>
    [DefaultValue("contact@supplier.com")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact phone number.
    /// </summary>
    [DefaultValue("+1-555-0100")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the supplier address.
    /// </summary>
    [DefaultValue("123 Industrial Blvd")]
    public string Address { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets the postal code.
    /// </summary>
    [DefaultValue("97201")]
    public string? PostalCode { get; set; }

    /// <summary>
    /// Gets or sets the supplier website.
    /// </summary>
    [DefaultValue("https://www.supplier.com")]
    public string? Website { get; set; }

    /// <summary>
    /// Gets or sets the credit limit for purchases.
    /// </summary>
    [DefaultValue(50000.00)]
    public decimal? CreditLimit { get; set; }

    /// <summary>
    /// Gets or sets the payment terms in days.
    /// </summary>
    [DefaultValue(30)]
    public int? PaymentTermsDays { get; set; }

    /// <summary>
    /// Gets or sets whether the supplier is active.
    /// </summary>
    [DefaultValue(true)]
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the supplier rating (0-5).
    /// </summary>
    [DefaultValue(0)]
    public decimal? Rating { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the image URL for the supplier logo.
    /// </summary>
    [DefaultValue(null)]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image is uploaded to storage and ImageUrl is set from the saved file name.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}

