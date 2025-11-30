using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain;

/// <summary>
/// Represents a product entity in the catalog domain.
/// Products belong to brands and have pricing information.
/// </summary>
public class Product : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>
    /// Maximum length for the product name field. (2^8 = 256)
    /// </summary>
    public const int NameMaxLength = 256;

    /// <summary>
    /// Maximum length for the product description field. (2^11 = 2048)
    /// </summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>
    /// Minimum length for the product name field.
    /// </summary>
    public const int NameMinLength = 2;

    /// <summary>
    /// Minimum price for a product.
    /// </summary>
    public const decimal MinPrice = 0.01m;

    /// <summary>
    /// Maximum price for a product.
    /// </summary>
    public const decimal MaxPrice = 999999999.99m;

    /// <summary>
    /// Gets the product price.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets the brand identifier this product belongs to.
    /// </summary>
    public DefaultIdType? BrandId { get; private set; }

    /// <summary>
    /// Gets the brand navigation property.
    /// </summary>
    public virtual Brand Brand { get; private set; } = default!;

    private Product() { }

    private Product(DefaultIdType id, string name, string? description, decimal price, DefaultIdType? brandId)
    {
        Id = id;
        ValidateAndSetName(name);
        ValidateAndSetDescription(description);
        ValidateAndSetPrice(price);
        BrandId = brandId;

        QueueDomainEvent(new ProductCreated { Product = this });
    }

    /// <summary>
    /// Creates a new Product instance using the factory method pattern.
    /// </summary>
    /// <param name="name">The product name (required, 2-256 characters).</param>
    /// <param name="description">Optional product description (max 2048 characters).</param>
    /// <param name="price">The product price (must be between 0.01 and 999999999.99).</param>
    /// <param name="brandId">Optional brand identifier.</param>
    /// <returns>A new Product instance.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    public static Product Create(string name, string? description, decimal price, DefaultIdType? brandId = null)
    {
        return new Product(DefaultIdType.NewGuid(), name, description, price, brandId);
    }

    /// <summary>
    /// Updates the product information with validation and change detection.
    /// Only raises domain events if actual changes are detected.
    /// </summary>
    /// <param name="name">The updated product name.</param>
    /// <param name="description">The updated product description.</param>
    /// <param name="price">The updated product price.</param>
    /// <param name="brandId">The updated brand identifier.</param>
    /// <returns>The updated Product instance.</returns>
    /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
    public Product Update(string? name, string? description, decimal? price, DefaultIdType? brandId)
    {
        // Track original values for event
        string originalName = Name;
        string? originalDescription = Description;
        decimal originalPrice = Price;
        DefaultIdType? originalBrandId = BrandId;
        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(name))
        {
            string trimmedName = name.Trim();
            if (!string.Equals(Name, trimmedName, StringComparison.OrdinalIgnoreCase))
            {
                ValidateAndSetName(trimmedName);
                hasChanges = true;
            }
        }

        // Handle description update (including null/empty to clear)
        string? trimmedDescription = description?.Trim();
        if (!string.Equals(Description, trimmedDescription, StringComparison.OrdinalIgnoreCase))
        {
            ValidateAndSetDescription(trimmedDescription);
            hasChanges = true;
        }

        if (price.HasValue && Price != price.Value)
        {
            ValidateAndSetPrice(price.Value);
            hasChanges = true;
        }

        if (brandId.HasValue && brandId.Value != DefaultIdType.Empty && BrandId != brandId.Value)
        {
            BrandId = brandId.Value;
            hasChanges = true;
        }

        // Only raise event if changes were detected
        if (hasChanges)
        {
            QueueDomainEvent(new ProductUpdated { Product = this });
        }

        return this;
    }

    /// <summary>
    /// Validates and sets the product name.
    /// </summary>
    /// <param name="name">The name to validate and set.</param>
    /// <exception cref="ArgumentException">Thrown when name is invalid.</exception>
    private void ValidateAndSetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name cannot be empty.", nameof(name));
        }

        string trimmedName = name.Trim();

        if (trimmedName.Length < NameMinLength)
        {
            throw new ArgumentException($"Product name must be at least {NameMinLength} characters.", nameof(name));
        }

        if (trimmedName.Length > NameMaxLength)
        {
            throw new ArgumentException($"Product name cannot exceed {NameMaxLength} characters.", nameof(name));
        }

        Name = trimmedName;
    }

    /// <summary>
    /// Validates and sets the product description.
    /// </summary>
    /// <param name="description">The description to validate and set.</param>
    /// <exception cref="ArgumentException">Thrown when description is invalid.</exception>
    private void ValidateAndSetDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            Description = null;
            return;
        }

        string trimmedDescription = description.Trim();

        if (trimmedDescription.Length > DescriptionMaxLength)
        {
            throw new ArgumentException($"Product description cannot exceed {DescriptionMaxLength} characters.", nameof(description));
        }

        Description = trimmedDescription;
    }

    /// <summary>
    /// Validates and sets the product price.
    /// </summary>
    /// <param name="price">The price to validate and set.</param>
    /// <exception cref="ArgumentException">Thrown when price is invalid.</exception>
    private void ValidateAndSetPrice(decimal price)
    {
        if (price < MinPrice)
        {
            throw new ArgumentException($"Product price must be at least {MinPrice}.", nameof(price));
        }

        if (price > MaxPrice)
        {
            throw new ArgumentException($"Product price cannot exceed {MaxPrice}.", nameof(price));
        }

        Price = price;
    }
}

