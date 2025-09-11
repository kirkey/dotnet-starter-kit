using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class Store : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Manager { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;

    // FK
    public DefaultIdType CompanyId { get; private set; }
    public Company Company { get; private set; } = default!;

    // Navigations
    public ICollection<Sale> Sales { get; private set; } = new List<Sale>();
    public ICollection<StoreTransfer> TransfersReceived { get; private set; } = new List<StoreTransfer>();

    private Store() { }

    private Store(string name, string code, string address, string phone, string manager, bool isActive, DefaultIdType companyId)
    {
        Name = name;
        Code = code;
        Address = address;
        Phone = phone;
        Manager = manager;
        IsActive = isActive;
        CompanyId = companyId;
    }

    public static Store Create(string name, string code, string address, string phone, string manager, bool isActive, DefaultIdType companyId)
        => new(name, code, address, phone, manager, isActive, companyId);

    public Store Update(string? name, string? code, string? address, string? phone, string? manager, bool? isActive, DefaultIdType? companyId)
    {
        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.Ordinal)) Name = name;
        if (!string.IsNullOrWhiteSpace(code) && !string.Equals(Code, code, StringComparison.Ordinal)) Code = code;
        if (address is not null && !string.Equals(Address, address, StringComparison.Ordinal)) Address = address;
        if (phone is not null && !string.Equals(Phone, phone, StringComparison.Ordinal)) Phone = phone;
        if (manager is not null && !string.Equals(Manager, manager, StringComparison.Ordinal)) Manager = manager;
        if (isActive.HasValue) IsActive = isActive.Value;
        if (companyId.HasValue && companyId.Value != DefaultIdType.Empty) CompanyId = companyId.Value;
        return this;
    }
}
