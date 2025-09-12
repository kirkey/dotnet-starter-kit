namespace Store.Domain.Exceptions.Category;

public sealed class CategoryInactiveException : Exception
{
    public CategoryInactiveException(DefaultIdType id)
        : base($"Category with ID '{id}' is inactive.") {}
}
