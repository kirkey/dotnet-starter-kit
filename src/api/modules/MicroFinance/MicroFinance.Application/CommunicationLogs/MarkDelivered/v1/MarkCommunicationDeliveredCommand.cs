using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.MarkDelivered.v1;

/// <summary>
/// Command to mark a communication as delivered.
/// </summary>
public sealed record MarkCommunicationDeliveredCommand(Guid LogId) : IRequest<MarkCommunicationDeliveredResponse>;
