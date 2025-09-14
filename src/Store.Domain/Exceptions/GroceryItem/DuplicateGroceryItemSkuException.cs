using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.GroceryItem;

public sealed class DuplicateGroceryItemSkuException(string sku)
    : BadRequestException($"A grocery item with SKU '{sku}' already exists.") {}

