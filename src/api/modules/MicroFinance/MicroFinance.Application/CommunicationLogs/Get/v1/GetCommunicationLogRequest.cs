using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Get.v1;

/// <summary>
/// Request to get a communication log by ID.
/// </summary>
public sealed record GetCommunicationLogRequest(DefaultIdType Id) : IRequest<CommunicationLogResponse>;
