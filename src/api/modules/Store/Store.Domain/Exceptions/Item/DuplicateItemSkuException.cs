namespace Store.Domain.Exceptions.Item;

public sealed class DuplicateItemSkuException(string sku)
    : BadRequestException($"An item with SKU '{sku}' already exists.") {}

