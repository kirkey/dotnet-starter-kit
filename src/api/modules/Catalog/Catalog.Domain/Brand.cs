using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Catalog.Domain.Events;

namespace FSH.Starter.WebApi.Catalog.Domain;
public class Brand : AuditableEntity, IAggregateRoot
{
    private Brand() { }

    private Brand(DefaultIdType id, string name, string? description, string? notes)
    {
        Id = id;
        Name = name;
        Description = description;
        Notes = notes;
        
        QueueDomainEvent(new BrandCreated { Brand = this });
    }

    public static Brand Create(string name, string? description, string? notes)
    {
        return new Brand(DefaultIdType.NewGuid(), name, description, notes);
    }

    public Brand Update(string? name, string? description, string? notes)
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

        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            Notes = notes;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new BrandUpdated { Brand = this });
        }

        return this;
    }
}


