namespace Accounting.Application.Checks.Void.v1;

/// <summary>
/// Command to void a check.
/// </summary>
public record CheckVoidCommand(
    DefaultIdType CheckId,
    string VoidReason,
    DateTime? VoidedDate
) : IRequest<CheckVoidResponse>;
