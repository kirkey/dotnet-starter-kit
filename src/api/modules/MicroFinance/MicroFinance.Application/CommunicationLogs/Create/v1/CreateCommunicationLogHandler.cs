using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Create.v1;

/// <summary>
/// Handler for creating a new communication log entry.
/// </summary>
public sealed class CreateCommunicationLogHandler(
    [FromKeyedServices("microfinance:communicationlogs")] IRepository<CommunicationLog> repository,
    ILogger<CreateCommunicationLogHandler> logger)
    : IRequestHandler<CreateCommunicationLogCommand, CreateCommunicationLogResponse>
{
    public async Task<CreateCommunicationLogResponse> Handle(CreateCommunicationLogCommand request, CancellationToken cancellationToken)
    {
        var communicationLog = CommunicationLog.Create(
            request.Channel,
            request.Recipient,
            request.Body,
            request.MemberId,
            request.LoanId,
            request.TemplateId,
            request.Subject,
            request.SentByUserId);

        await repository.AddAsync(communicationLog, cancellationToken);

        logger.LogInformation("Communication log created - Channel: {Channel}, Recipient: {Recipient}",
            request.Channel, request.Recipient);

        return new CreateCommunicationLogResponse(
            communicationLog.Id,
            communicationLog.Channel,
            communicationLog.Recipient,
            communicationLog.DeliveryStatus);
    }
}
