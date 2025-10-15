namespace Accounting.Application.Checks.Print.v1;

/// <summary>
/// Response after marking check as printed.
/// </summary>
public record CheckPrintResponse(DefaultIdType Id, string CheckNumber, bool IsPrinted);

