namespace Accounting.Application.AccountsPayableAccounts.Exceptions;

/// <summary>
/// Exception thrown when an accounts payable account is not found.
/// </summary>
public class AccountsPayableAccountNotFoundException(DefaultIdType id)
    : FshException($"Accounts payable account with ID {id} was not found.");

/// <summary>
/// Exception thrown when attempting to create a duplicate account number.
/// </summary>
public class DuplicateApAccountNumberException(string accountNumber)
    : ConflictException($"Accounts payable account with number '{accountNumber}' already exists.");

/// <summary>
/// Exception thrown when attempting to delete an account with outstanding balance.
/// </summary>
public class ApAccountHasOutstandingBalanceException(DefaultIdType id)
    : BadRequestException($"Cannot delete accounts payable account with ID {id} because it has an outstanding balance.");

