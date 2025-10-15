namespace Accounting.Application.Checks.Clear.v1;

/// <summary>
/// Response after clearing a check.
/// </summary>
public record CheckClearResponse(DefaultIdType Id, string CheckNumber, string Status);

