namespace Store.Domain.Exceptions.Item;

public sealed class DuplicateItemBarcodeException(string barcode)
    : BadRequestException($"An item with Barcode '{barcode}' already exists.") {}

