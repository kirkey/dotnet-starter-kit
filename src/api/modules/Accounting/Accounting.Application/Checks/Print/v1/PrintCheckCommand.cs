namespace Accounting.Application.Checks.Print.v1;

/// <summary>
/// Command to mark a check as printed.
/// </summary>
public record PrintCheckCommand(
    DefaultIdType CheckId,
    string PrintedBy,
    DateTime? PrintedDate
) : IRequest<CheckPrintResponse>;

