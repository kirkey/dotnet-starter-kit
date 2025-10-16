namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a bank is not found by its ID.
/// </summary>
public class BankNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankNotFoundException"/> class.
    /// </summary>
    /// <param name="id">The bank ID that was not found.</param>
    public BankNotFoundException(DefaultIdType id)
        : base($"Bank with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when a bank is not found by its bank code.
/// </summary>
public class BankByCodeNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankByCodeNotFoundException"/> class.
    /// </summary>
    /// <param name="bankCode">The bank code that was not found.</param>
    public BankByCodeNotFoundException(string bankCode)
        : base($"Bank with code '{bankCode}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to create a bank with a code that already exists.
/// </summary>
public class BankCodeAlreadyExistsException : ConflictException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankCodeAlreadyExistsException"/> class.
    /// </summary>
    /// <param name="bankCode">The duplicate bank code.</param>
    public BankCodeAlreadyExistsException(string bankCode)
        : base($"Bank with code '{bankCode}' already exists.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to create a bank with a routing number that already exists.
/// </summary>
public class BankRoutingNumberAlreadyExistsException : ConflictException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankRoutingNumberAlreadyExistsException"/> class.
    /// </summary>
    /// <param name="routingNumber">The duplicate routing number.</param>
    public BankRoutingNumberAlreadyExistsException(string routingNumber)
        : base($"Bank with routing number '{routingNumber}' already exists.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to deactivate a bank that has active bank accounts.
/// </summary>
public class BankHasActiveBankAccountsException : ConflictException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankHasActiveBankAccountsException"/> class.
    /// </summary>
    /// <param name="bankCode">The bank code.</param>
    public BankHasActiveBankAccountsException(string bankCode)
        : base($"Cannot deactivate bank '{bankCode}' because it has active bank accounts.")
    {
    }
}

/// <summary>
/// Exception thrown when attempting to delete a bank that has associated bank accounts.
/// </summary>
public class BankHasBankAccountsException : ConflictException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankHasBankAccountsException"/> class.
    /// </summary>
    /// <param name="bankCode">The bank code.</param>
    public BankHasBankAccountsException(string bankCode)
        : base($"Cannot delete bank '{bankCode}' because it has associated bank accounts. Deactivate instead.")
    {
    }
}

