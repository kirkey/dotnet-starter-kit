using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.Category;

public sealed class CategoryNotFoundByCodeException(string code)
    : NotFoundException($"Category with Code '{code}' was not found.") {}
