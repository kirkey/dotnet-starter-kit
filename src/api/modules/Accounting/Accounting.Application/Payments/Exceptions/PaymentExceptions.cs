namespace Accounting.Application.Payments.Exceptions;

/// <summary>
/// Exception thrown when a payment number already exists in the system.
/// </summary>
public class PaymentNumberAlreadyExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentNumberAlreadyExistsException"/> class.
    /// </summary>
    /// <param name="paymentNumber">The payment number that already exists.</param>
    public PaymentNumberAlreadyExistsException(string paymentNumber)
        : base($"Payment number '{paymentNumber}' already exists.")
    {
        PaymentNumber = paymentNumber;
    }

    /// <summary>
    /// Gets the payment number that already exists.
    /// </summary>
    public string PaymentNumber { get; }
}

/// <summary>
/// Exception thrown when a payment is not found.
/// </summary>
public class PaymentNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentNotFoundException"/> class.
    /// </summary>
    /// <param name="paymentId">The payment ID that was not found.</param>
    public PaymentNotFoundException(DefaultIdType paymentId)
        : base($"Payment with ID '{paymentId}' was not found.")
    {
        PaymentId = paymentId;
    }

    /// <summary>
    /// Gets the payment ID that was not found.
    /// </summary>
    public DefaultIdType PaymentId { get; }
}

