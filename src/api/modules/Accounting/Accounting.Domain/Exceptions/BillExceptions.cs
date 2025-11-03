namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a bill is not found.
/// </summary>
public sealed class BillNotFoundException : NotFoundException
{
    public BillNotFoundException(DefaultIdType billId)
        : base($"Bill with ID '{billId}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to modify a bill that cannot be modified.
/// </summary>
public sealed class BillCannotBeModifiedException : BadRequestException
{
    public BillCannotBeModifiedException(DefaultIdType billId, string reason)
        : base($"Bill '{billId}' cannot be modified: {reason}")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to post a bill that is already posted.
/// </summary>
public sealed class BillAlreadyPostedException : BadRequestException
{
    public BillAlreadyPostedException(DefaultIdType billId)
        : base($"Bill '{billId}' is already posted to the general ledger.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to approve a bill that is already approved.
/// </summary>
public sealed class BillAlreadyApprovedException : BadRequestException
{
    public BillAlreadyApprovedException(DefaultIdType billId)
        : base($"Bill '{billId}' is already approved.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to post a bill that is not approved.
/// </summary>
public sealed class BillNotApprovedException : BadRequestException
{
    public BillNotApprovedException(DefaultIdType billId)
        : base($"Bill '{billId}' must be approved before it can be posted.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to mark a bill as paid that is not posted.
/// </summary>
public sealed class BillNotPostedException : BadRequestException
{
    public BillNotPostedException(DefaultIdType billId)
        : base($"Bill '{billId}' must be posted before it can be marked as paid.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to pay a bill that is already paid.
/// </summary>
public sealed class BillAlreadyPaidException : BadRequestException
{
    public BillAlreadyPaidException(DefaultIdType billId)
        : base($"Bill '{billId}' is already marked as paid.")
    {
    }
}

/// <summary>
/// Exception thrown when a bill has an invalid amount.
/// </summary>
public sealed class BillInvalidAmountException : BadRequestException
{
    public BillInvalidAmountException(DefaultIdType billId, string reason)
        : base($"Bill '{billId}' has an invalid amount: {reason}")
    {
    }
}

/// <summary>
/// Exception thrown when a bill line item is not found.
/// </summary>
public sealed class BillLineItemNotFoundException : NotFoundException
{
    public BillLineItemNotFoundException(DefaultIdType lineItemId)
        : base($"Bill line item with ID '{lineItemId}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to add a line item to a bill that cannot be modified.
/// </summary>
public sealed class BillLineItemCannotBeAddedException : BadRequestException
{
    public BillLineItemCannotBeAddedException(DefaultIdType billId)
        : base($"Cannot add line items to bill '{billId}' because it is posted or paid.")
    {
    }
}

