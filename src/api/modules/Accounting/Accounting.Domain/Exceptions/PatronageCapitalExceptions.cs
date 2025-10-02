// Patronage Capital Exceptions

using FSH.Framework.Core.Exceptions;

namespace Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when patronage capital is not found by ID.
/// </summary>
public sealed class PatronageCapitalByIdNotFoundException(DefaultIdType id) : NotFoundException($"patronage capital with id {id} not found");

/// <summary>
/// Exception thrown when patronage capital is not found by member and fiscal year.
/// </summary>
public sealed class PatronageCapitalByMemberAndYearNotFoundException(DefaultIdType memberId, int fiscalYear) : NotFoundException($"patronage capital for member {memberId} in fiscal year {fiscalYear} not found");

/// <summary>
/// Exception thrown when trying to allocate patronage capital for a duplicate member/year combination.
/// </summary>
public sealed class DuplicatePatronageCapitalAllocationException(DefaultIdType memberId, int fiscalYear) : ConflictException($"patronage capital allocation for member {memberId} in fiscal year {fiscalYear} already exists");

/// <summary>
/// Exception thrown when patronage capital allocation amount is invalid.
/// </summary>
public sealed class InvalidPatronageCapitalAmountException() : ForbiddenException("patronage capital allocation amount must be positive");

/// <summary>
/// Exception thrown when trying to retire more than the available capital.
/// </summary>
public sealed class InsufficientPatronageCapitalException(decimal availableAmount, decimal requestedAmount) 
    : ForbiddenException($"cannot retire {requestedAmount:C}. Only {availableAmount:C} available for retirement");

/// <summary>
/// Exception thrown when the fiscal year is invalid.
/// </summary>
public sealed class InvalidFiscalYearException(int fiscalYear) : ForbiddenException($"fiscal year {fiscalYear} cannot be in the future");

/// <summary>
/// Exception thrown when trying to modify patronage capital that is fully retired.
/// </summary>
public sealed class CannotModifyRetiredPatronageCapitalException(DefaultIdType id) : ForbiddenException($"cannot modify fully retired patronage capital with id {id}");

/// <summary>
/// Exception thrown when the member is not eligible for patronage capital allocation.
/// </summary>
public sealed class MemberNotEligibleForPatronageCapitalException(DefaultIdType memberId) : ForbiddenException($"member {memberId} is not eligible for patronage capital allocation");
