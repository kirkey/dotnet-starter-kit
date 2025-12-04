namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Get.v1;

/// <summary>
/// Response containing communication log details.
/// </summary>
public sealed record CommunicationLogResponse(
    Guid Id,
    Guid? MemberId,
    Guid? LoanId,
    string Channel,
    string Recipient,
    string? Subject,
    string Body,
    string DeliveryStatus,
    DateTime? SentAt,
    DateTime? DeliveredAt,
    int RetryCount,
    string? ErrorMessage,
    decimal? Cost);
