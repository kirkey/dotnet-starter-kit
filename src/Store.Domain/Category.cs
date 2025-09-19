namespace Store.Domain;

/// <summary>
/// Represents a product category used to organize and group grocery items with hierarchical navigation and business rule management.
/// </summary>
/// <remarks>
/// Use cases:
/// - Organize products into logical hierarchies for customer navigation and browsing experience.
/// - Support multi-level category trees with parent-child relationships for complex product taxonomies.
/// - Enable category-specific pricing rules, promotions, and discount strategies.
/// - Facilitate inventory reporting and analysis by product category groupings.
/// - Support category-based search and filtering for improved product discovery.
/// - Enable category-specific business rules like tax rates, shipping restrictions, or age verification.
/// - Manage seasonal categories and promotional product groupings.
/// - Support category-based inventory planning and procurement strategies.
/// 
/// Default values:
/// - Code: required unique identifier, max 50 characters (example: "FRUITS", "DAIRY", "MEAT")
/// - ParentCategoryId: null for top-level categories (example: "Food" parent for "Fruits" subcategory)
/// - IsActive: true (new categories are active by default)
/// - SortOrder: 0 (categories with lower numbers appear first in listings)
/// - ImageUrl: null (optional category image for display purposes)
/// - Name: inherited display name (example: "Fresh Fruits", "Dairy Products")
/// - Description: inherited detailed description (example: "Fresh seasonal fruits and produce")
/// - TaxRate: null (optional category-specific tax rate override)
/// - RequiresAgeVerification: false (true for alcohol, tobacco, etc.)
/// - IsPerishable: false (true for categories requiring expiration tracking)
/// 
/// Business rules:
/// - Code must be unique within the system
/// - Cannot create circular parent-child relationships
/// - Cannot deactivate categories with active subcategories
/// - Cannot delete categories with assigned products
/// - Parent category must exist and be active
/// - Category hierarchy depth should be limited (typically 3-4 levels)
/// - Sort order determines display sequence within parent level
/// - Image URL must be valid if specified
/// - Category codes should follow naming conventions
/// </remarks>
/// <seealso cref="Store.Domain.Events.CategoryCreated"/>
/// <seealso cref="Store.Domain.Events.CategoryUpdated"/>
/// <seealso cref="Store.Domain.Events.CategoryActivated"/>
/// <seealso cref="Store.Domain.Events.CategoryDeactivated"/>
/// <seealso cref="Store.Domain.Events.CategoryHierarchyChanged"/>
/// <seealso cref="Store.Domain.Exceptions.Category.CategoryNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.Category.CategoryNotFoundByCodeException"/>
/// <seealso cref="Store.Domain.Exceptions.Category.CategoryInactiveException"/>
/// <seealso cref="Store.Domain.Exceptions.Category.DuplicateCategoryCodeException"/>
/// <seealso cref="Store.Domain.Exceptions.Category.CircularCategoryReferenceException"/>
public sealed class Category : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Short unique code for the category. Example: "FRUITS" or "CAT-001".
    /// Max length: 50.
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Optional parent category id for hierarchical categories. null means top-level.
    /// Example: null for "Food", a Food category Id for "Fruits".
    /// </summary>
    public DefaultIdType? ParentCategoryId { get; private set; }

    /// <summary>
    /// Whether the category is visible/active in the system. Default: true.
    /// Used to hide categories without deleting them.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Ordering index used when listing categories. Lower numbers appear first. Default: 0.
    /// Example: 10 for "Fruits", 20 for "Vegetables".
    /// </summary>
    public int SortOrder { get; private set; }

    /// <summary>
    /// Optional URL to a representative image for the category. Example: "https://cdn.example.com/images/fruits.png".
    /// Max length: 500.
    /// </summary>
    public string? ImageUrl { get; private set; }
    
    /// <summary>
    /// Navigation property to parent category if this is a subcategory.
    /// </summary>
    public Category? ParentCategory { get; private set; }

    /// <summary>
    /// Navigation property to child subcategories.
    /// Example count: 0 for leaf categories, 5+ for major categories.
    /// </summary>
    public ICollection<Category> SubCategories { get; private set; } = new List<Category>();

    /// <summary>
    /// Navigation property to grocery items in this category.
    /// Example count: varies widely by category breadth.
    /// </summary>
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

    /// <summary>
    /// Creates a new category with the specified attributes.
    /// </summary>
    /// <param name="name">The name of the category. Must be between 1 and 200 characters.</param>
    /// <param name="description">An optional description of the category.</param>
    /// <param name="code">A unique code for the category. Must be between 1 and 50 characters.</param>
    /// <param name="parentCategoryId">An optional parent category ID for hierarchical categories.</param>
    /// <param name="isActive">Whether the category is active. Default is true.</param>
    /// <param name="sortOrder">The sort order of the category. Must be 0 or greater. Default is 0.</param>
    /// <param name="imageUrl">An optional URL to an image representing the category.</param>
    /// <returns>A new <see cref="Category"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when validation for any argument fails.</exception>
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

    /// <summary>
    /// Updates the category with the specified attributes.
    /// </summary>
    /// <param name="name">The new name of the category.</param>
    /// <param name="description">The new description of the category.</param>
    /// <param name="code">The new code for the category.</param>
    /// <param name="parentCategoryId">The new parent category ID.</param>
    /// <param name="isActive">Whether the category is active.</param>
    /// <param name="sortOrder">The new sort order of the category.</param>
    /// <param name="imageUrl">The new image URL for the category.</param>
    /// <returns>The updated <see cref="Category"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when validation for any argument fails.</exception>
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

    /// <summary>
    /// Activates the category, making it visible in the system.
    /// </summary>
    /// <returns>The updated <see cref="Category"/> instance.</returns>
    public Category Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new CategoryActivated { Category = this });
        }
        return this;
    }

    /// <summary>
    /// Deactivates the category, hiding it from the system.
    /// </summary>
    /// <returns>The updated <see cref="Category"/> instance.</returns>
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
