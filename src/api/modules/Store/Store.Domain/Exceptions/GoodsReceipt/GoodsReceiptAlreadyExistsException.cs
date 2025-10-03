namespace Store.Domain.Exceptions.GoodsReceipt;

/// <summary>
/// Exception thrown when attempting to create a goods receipt with a number that already exists.
/// </summary>
public sealed class GoodsReceiptAlreadyExistsException : Exception
{
    public GoodsReceiptAlreadyExistsException(string receiptNumber)
        : base($"Goods receipt with number '{receiptNumber}' already exists.")
    {
        ReceiptNumber = receiptNumber;
    }

    public string ReceiptNumber { get; }
}
