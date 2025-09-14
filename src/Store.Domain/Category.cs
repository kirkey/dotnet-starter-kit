namespace Store.Domain;

public sealed class Category : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = default!;
    public DefaultIdType? ParentCategoryId { get; private set; }
    public bool IsActive { get; private set; } = true;
    public int SortOrder { get; private set; }
    public string? ImageUrl { get; private set; }
    
    public Category? ParentCategory { get; private set; }
    public ICollection<Category> SubCategories { get; private set; } = new List<Category>();
    public ICollection<GroceryItem> GroceryItems { get; private set; } = new List<GroceryItem>();

    private Category() { }

    private Category(
        DefaultIdType id,
        string name,
        string? description,
        string code,
        DefaultIdType? parentCategoryId,
        bool isActive,
        int sortOrder,
        string? imageUrl)
    {
        // domain validations
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required", nameof(code));
        if (code.Length > 50) throw new ArgumentException("Code must not exceed 50 characters", nameof(code));

        if (sortOrder < 0) throw new ArgumentException("SortOrder must be zero or greater", nameof(sortOrder));

        if (imageUrl is { Length: > 500 }) throw new ArgumentException("ImageUrl must not exceed 500 characters", nameof(imageUrl));

        Id = id;
        Name = name;
        Description = description;
        Code = code;
        ParentCategoryId = parentCategoryId;
        IsActive = isActive;
        SortOrder = sortOrder;
        ImageUrl = imageUrl;

        QueueDomainEvent(new CategoryCreated { Category = this });
    }

    public static Category Create(
        string name,
        string? description,
        string code,
        DefaultIdType? parentCategoryId = null,
        bool isActive = true,
        int sortOrder = 0,
        string? imageUrl = null)
    {
        return new Category(
            DefaultIdType.NewGuid(),
            name,
            description,
            code,
            parentCategoryId,
            isActive,
            sortOrder,
            imageUrl);
    }

    public Category Update(
        string? name,
        string? description,
        string? code,
        DefaultIdType? parentCategoryId,
        bool? isActive,
        int? sortOrder,
        string? imageUrl)
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

        if (!string.IsNullOrWhiteSpace(code) && !string.Equals(Code, code, StringComparison.OrdinalIgnoreCase))
        {
            Code = code;
            isUpdated = true;
        }

        if (ParentCategoryId != parentCategoryId)
        {
            ParentCategoryId = parentCategoryId;
            isUpdated = true;
        }

        if (isActive.HasValue && IsActive != isActive.Value)
        {
            IsActive = isActive.Value;
            isUpdated = true;
        }

        if (sortOrder.HasValue && SortOrder != sortOrder.Value)
        {
            SortOrder = sortOrder.Value;
            isUpdated = true;
        }

        if (!string.Equals(ImageUrl, imageUrl, StringComparison.OrdinalIgnoreCase))
        {
            ImageUrl = imageUrl;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new CategoryUpdated { Category = this });
        }

        return this;
    }

    public Category Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new CategoryActivated { Category = this });
        }
        return this;
    }

    public Category Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new CategoryDeactivated { Category = this });
        }
        return this;
    }
}
