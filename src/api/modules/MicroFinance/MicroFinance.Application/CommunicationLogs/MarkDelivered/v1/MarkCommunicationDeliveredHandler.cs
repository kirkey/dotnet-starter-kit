using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.MarkDelivered.v1;

/// <summary>
/// Handler for marking a communication as delivered.
/// </summary>
public sealed class MarkCommunicationDeliveredHandler(
    [FromKeyedServices("microfinance:communicationlogs")] IRepository<CommunicationLog> repository,
    ILogger<MarkCommunicationDeliveredHandler> logger)
    : IRequestHandler<MarkCommunicationDeliveredCommand, MarkCommunicationDeliveredResponse>
{
    public async Task<MarkCommunicationDeliveredResponse> Handle(MarkCommunicationDeliveredCommand request, CancellationToken cancellationToken)
    {
        var communicationLog = await repository.GetByIdAsync(request.LogId, cancellationToken);

        if (communicationLog is null)
        {
            throw new NotFoundException($"Communication log with ID {request.LogId} not found.");
        }

        communicationLog.MarkDelivered();
        await repository.UpdateAsync(communicationLog, cancellationToken);

        logger.LogInformation("Communication {LogId} marked as delivered", request.LogId);

        return new MarkCommunicationDeliveredResponse(
            communicationLog.Id,
            communicationLog.DeliveryStatus,
            communicationLog.DeliveredAt!.Value);
    }
}
