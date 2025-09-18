namespace Store.Domain.Exceptions.Category;

public sealed class CategoryInactiveException(DefaultIdType id) : Exception($"Category with ID '{id}' is inactive.");
