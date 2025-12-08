using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Get.v1;

/// <summary>
/// Specification for getting a communication log by ID.
/// </summary>
public sealed class CommunicationLogByIdSpec : Specification<CommunicationLog, CommunicationLogResponse>
{
    public CommunicationLogByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);

        Query.Select(c => new CommunicationLogResponse(
            c.Id,
            c.MemberId,
            c.LoanId,
            c.Channel,
            c.Recipient,
            c.Subject,
            c.Body,
            c.DeliveryStatus,
            c.SentAt,
            c.DeliveredAt,
            c.RetryCount,
            c.ErrorMessage,
            c.Cost));
    }
}
