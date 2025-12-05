namespace Accounting.Application.Checks.Create.v1;

/// <summary>
/// Command to register a bundle of new checks in the system.
/// Checks come in pre-printed pads/books with sequential numbers (e.g., 3453000-3453500).
/// This command creates all checks in the specified range at once.
/// </summary>
/// <remarks>
/// Bundle Creation:
/// - StartCheckNumber: The first check number in the range (e.g., "3453000")
/// - EndCheckNumber: The last check number in the range (e.g., "3453500")
/// - Creates all sequential check numbers from StartCheckNumber to EndCheckNumber inclusive
/// 
/// Auto-Fetching Behavior:
/// - BankAccountCode: Used to look up ChartOfAccount and retrieve BankAccountName
/// - BankId: If provided, used to look up Bank and retrieve BankName
/// - BankAccountName and BankName are automatically populated from their respective lookups
/// 
/// Applied to all checks in the bundle.
/// </remarks>
public record CreateCheckCommand(
    string StartCheckNumber,
    string EndCheckNumber,
    string BankAccountCode,
    DefaultIdType? BankId,
    string? Description,
    string? Notes
) : IRequest<CheckCreateResponse>;

