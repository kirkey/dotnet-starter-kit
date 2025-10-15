namespace Accounting.Application.Checks.Void.v1;

/// <summary>
/// Response after voiding a check.
/// </summary>
public record CheckVoidResponse(DefaultIdType Id, string CheckNumber, string Status);

