// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/Warehouse/Domain/Category.cs
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class Category : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = string.Empty;
    public string? DescriptionText { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation (optional inverse)
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    private Category() { }

    private Category(string name, string code, string? description, bool isActive)
    {
        Name = name;
        Code = code;
        DescriptionText = description;
        IsActive = isActive;
    }

    public static Category Create(string name, string code, string? description, bool isActive = true)
        => new(name, code, description, isActive);

    public Category Update(string? name, string? code, string? description, bool? isActive)
    {
        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.Ordinal)) Name = name;
        if (!string.IsNullOrWhiteSpace(code) && !string.Equals(Code, code, StringComparison.Ordinal)) Code = code;
        if (description is not null && !string.Equals(DescriptionText, description, StringComparison.Ordinal)) DescriptionText = description;
        if (isActive.HasValue) IsActive = isActive.Value;
        return this;
    }
}

