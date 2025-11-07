namespace Accounting.Application.GeneralLedgers.Update.v1;

/// <summary>
/// Command to update a general ledger entry.
/// </summary>
/// <remarks>
/// Update Rules:
/// - Can update amounts, memo, USOA class, and reference information
/// - Cannot change EntryId or AccountId (that would be a different GL entry)
/// - Debit and Credit must remain non-negative
/// - Used for adjustments and corrections
/// 
/// Business Rules:
/// - Should only be used for corrections before period close
/// - Consider creating a reversing entry instead for auditing purposes
/// - USOA class must be valid
/// - At least one field must be provided for update
/// </remarks>
public sealed record GeneralLedgerUpdateCommand(
    DefaultIdType Id,
    decimal? Debit = null,
    decimal? Credit = null,
    string? Memo = null,
    string? UsoaClass = null,
    string? ReferenceNumber = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;
