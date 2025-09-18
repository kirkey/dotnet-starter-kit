// PayeeExceptions.cs
// Per-domain exceptions for Payees



namespace Accounting.Domain.Exceptions;

public sealed class PayeeNotFoundException(DefaultIdType id) : NotFoundException($"payee with id {id} not found");

// Add more Payee-related exceptions here as needed

