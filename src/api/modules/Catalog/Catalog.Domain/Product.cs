using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain;
public class Product : AuditableEntity, IAggregateRoot
{
    public decimal Price { get; private set; }
    public DefaultIdType? BrandId { get; private set; }
    public virtual Brand Brand { get; private set; } = default!;

    private Product() { }

    private Product(DefaultIdType id, string name, string? description, decimal price, DefaultIdType? brandId)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        BrandId = brandId;

        QueueDomainEvent(new ProductCreated { Product = this });
    }

    public static Product Create(string name, string? description, decimal price, DefaultIdType? brandId)
    {
        return new Product(DefaultIdType.NewGuid(), name, description, price, brandId);
    }

    public Product Update(string? name, string? description, decimal? price, DefaultIdType? brandId)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (price.HasValue && Price != price.Value)
        {
            Price = price.Value;
            isUpdated = true;
        }

        if (brandId.HasValue && brandId.Value != DefaultIdType.Empty && BrandId != brandId.Value)
        {
            BrandId = brandId.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ProductUpdated { Product = this });
        }

        return this;
    }
}

