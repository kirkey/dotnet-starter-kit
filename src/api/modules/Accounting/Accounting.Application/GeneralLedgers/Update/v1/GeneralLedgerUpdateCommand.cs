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
public sealed record GeneralLedgerUpdateCommand : IRequest<DefaultIdType>
{
    /// <summary>
    /// The ID of the general ledger entry to update.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// Updated debit amount (if applicable).
    /// </summary>
    public decimal? Debit { get; init; }
    
    /// <summary>
    /// Updated credit amount (if applicable).
    /// </summary>
    public decimal? Credit { get; init; }
    
    /// <summary>
    /// Updated memo/description.
    /// </summary>
    public string? Memo { get; init; }
    
    /// <summary>
    /// Updated USOA classification.
    /// </summary>
    public string? UsoaClass { get; init; }
    
    /// <summary>
    /// Updated reference number.
    /// </summary>
    public string? ReferenceNumber { get; init; }
    
    /// <summary>
    /// Updated description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Updated notes.
    /// </summary>
    public string? Notes { get; init; }
}
