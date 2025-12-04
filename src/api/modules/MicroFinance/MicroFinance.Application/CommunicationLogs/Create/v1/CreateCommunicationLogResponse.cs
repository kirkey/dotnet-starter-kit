namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Create.v1;

/// <summary>
/// Response after creating a communication log.
/// </summary>
public sealed record CreateCommunicationLogResponse(
    Guid Id,
    string Channel,
    string Recipient,
    string DeliveryStatus);
