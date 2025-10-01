namespace Store.Domain.Exceptions.Category;

public sealed class CategoryNotFoundException(DefaultIdType id)
    : NotFoundException($"Category with ID '{id}' was not found.") {}
