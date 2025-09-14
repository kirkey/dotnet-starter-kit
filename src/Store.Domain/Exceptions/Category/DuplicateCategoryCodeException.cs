namespace Store.Domain.Exceptions.Category;

public sealed class DuplicateCategoryCodeException(string code)
    : ConflictException($"Category with Code '{code}' already exists.") {}
