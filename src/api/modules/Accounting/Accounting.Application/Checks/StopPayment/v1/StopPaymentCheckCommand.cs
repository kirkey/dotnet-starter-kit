namespace Accounting.Application.Checks.StopPayment.v1;

/// <summary>
/// Command to request stop payment on a check.
/// </summary>
public record StopPaymentCheckCommand(
    DefaultIdType CheckId,
    string StopPaymentReason,
    DateTime? StopPaymentDate
) : IRequest<CheckStopPaymentResponse>;
