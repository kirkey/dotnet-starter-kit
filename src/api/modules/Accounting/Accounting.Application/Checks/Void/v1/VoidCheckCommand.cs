namespace Accounting.Application.Checks.Void.v1;

/// <summary>
/// Command to void a check.
/// </summary>
public record VoidCheckCommand(
    DefaultIdType CheckId,
    string VoidReason,
    DateTime? VoidedDate
) : IRequest<CheckVoidResponse>;
