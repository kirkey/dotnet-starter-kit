namespace Accounting.Application.AccountingPeriods.Delete.v1;

/// <summary>
/// Command to delete an accounting period. Use with caution — deletion removes the period record.
/// </summary>
public class DeleteAccountingPeriodCommand(DefaultIdType id) : IRequest
{
    /// <summary>
    /// The identifier of the accounting period to delete.
    /// </summary>
    public DefaultIdType Id { get; set; } = id;
}
