namespace Store.Domain.Exceptions.Category;

public sealed class CategoryInactiveException(DefaultIdType id) : CustomException($"Category with ID '{id}' is inactive.");
