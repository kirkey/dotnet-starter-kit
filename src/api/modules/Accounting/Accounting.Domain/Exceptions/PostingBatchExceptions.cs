// Posting Batch Exceptions

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when a posting batch is not found by ID.
/// </summary>
public sealed class PostingBatchByIdNotFoundException(DefaultIdType id) : NotFoundException($"posting batch with id {id} not found");

/// <summary>
/// Exception thrown when a posting batch is not found by batch number.
/// </summary>
public sealed class PostingBatchByNumberNotFoundException(string batchNumber) : NotFoundException($"posting batch with number {batchNumber} not found");

/// <summary>
/// Exception thrown when trying to create a posting batch with a duplicate number.
/// </summary>
public sealed class DuplicatePostingBatchNumberException(string batchNumber) : ConflictException($"posting batch with number {batchNumber} already exists");

/// <summary>
/// Exception thrown when trying to modify a posted batch.
/// </summary>
public sealed class CannotModifyPostedBatchException(DefaultIdType id) : ForbiddenException($"cannot modify posted batch with id {id}");

/// <summary>
/// Exception thrown when a posting batch is out of balance.
/// </summary>
public sealed class PostingBatchOutOfBalanceException(decimal debitTotal, decimal creditTotal) 
    : ForbiddenException($"posting batch is out of balance. Debits: {debitTotal:C}, Credits: {creditTotal:C}");

/// <summary>
/// Exception thrown when trying to post an empty batch.
/// </summary>
public sealed class CannotPostEmptyBatchException() : ForbiddenException("cannot post an empty batch");

/// <summary>
/// Exception thrown when the posting date is invalid.
/// </summary>
public sealed class InvalidPostingDateException() : ForbiddenException("posting date cannot be in the future");

/// <summary>
/// Exception thrown when trying to reverse an already reversed batch.
/// </summary>
public sealed class PostingBatchAlreadyReversedException(DefaultIdType id) : ForbiddenException($"posting batch with id {id} is already reversed");
