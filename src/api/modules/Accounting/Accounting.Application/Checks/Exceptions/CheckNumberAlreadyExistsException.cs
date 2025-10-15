namespace Accounting.Application.Checks.Exceptions;

/// <summary>
/// Exception thrown when a check number already exists for a bank account.
/// </summary>
public class CheckNumberAlreadyExistsException(string checkNumber, string bankAccountCode)
    : Exception($"Check number '{checkNumber}' already exists for bank account '{bankAccountCode}'.");

