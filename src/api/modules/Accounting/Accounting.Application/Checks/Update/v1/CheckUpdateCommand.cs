namespace Accounting.Application.Checks.Update.v1;

/// <summary>
/// Command to update an existing check in the system.
/// </summary>
/// <remarks>
/// Only available checks can be updated.
/// Auto-Fetching Behavior:
/// - BankAccountCode: Used to look up ChartOfAccount and retrieve BankAccountName
/// - BankId: If provided, used to look up Bank and retrieve BankName
/// - BankAccountName and BankName are automatically populated from their respective lookups
/// </remarks>
public record CheckUpdateCommand(
    DefaultIdType CheckId,
    string CheckNumber,
    string BankAccountCode,
    DefaultIdType? BankId,
    string? Description,
    string? Notes
) : IRequest<CheckUpdateResponse>;
