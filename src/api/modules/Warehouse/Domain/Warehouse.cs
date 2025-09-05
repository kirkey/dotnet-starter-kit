using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.Warehouse.Domain.Events;
using FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public sealed class Warehouse : AuditableEntity, IAggregateRoot
{
    private Warehouse() { }

    private Warehouse(string name, string code, Address address, string? description = null)
    {
        Name = name;
        Code = code;
        Address = address;
        Description = description;
        IsActive = true;
        Capacity = new Capacity();

        QueueDomainEvent(new WarehouseCreated(Id, Name, Code, Address.ToString()));
        WarehouseMetrics.Created.Add(1);
    }

    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public Capacity Capacity { get; private set; } = default!;

    public static Warehouse Create(string name, string code, Address address, string? description = null) =>
        new(name, code, address, description);

    public Warehouse Update(string? name, string? description, Address? address)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (description != null && !string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (address != null && !Address.Equals(address))
        {
            Address = address;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new WarehouseUpdated(this));
            WarehouseMetrics.Updated.Add(1);
        }

        return this;
    }

    public void Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new WarehouseDeactivated(Id, Name, Code));
            WarehouseMetrics.Deactivated.Add(1);
        }
    }

    public void Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new WarehouseActivated(Id, Name, Code));
            WarehouseMetrics.Activated.Add(1);
        }
    }
}
