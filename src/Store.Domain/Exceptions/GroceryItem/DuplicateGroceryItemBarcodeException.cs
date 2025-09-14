using FSH.Framework.Core.Exceptions;

namespace Store.Domain.Exceptions.GroceryItem;

public sealed class DuplicateGroceryItemBarcodeException(string barcode)
    : BadRequestException($"A grocery item with Barcode '{barcode}' already exists.") {}

