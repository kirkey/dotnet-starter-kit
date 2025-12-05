namespace Accounting.Application.Checks.Clear.v1;

/// <summary>
/// Command to mark a check as cleared.
/// </summary>
public record ClearCheckCommand(
    DefaultIdType CheckId,
    DateTime? ClearedDate
) : IRequest<CheckClearResponse>;

