namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.MarkDelivered.v1;

/// <summary>
/// Response after marking a communication as delivered.
/// </summary>
public sealed record MarkCommunicationDeliveredResponse(Guid Id, string DeliveryStatus, DateTime DeliveredAt);
