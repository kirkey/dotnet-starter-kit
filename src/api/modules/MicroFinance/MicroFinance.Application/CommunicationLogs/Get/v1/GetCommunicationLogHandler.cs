using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Get.v1;

/// <summary>
/// Handler for getting a communication log by ID.
/// </summary>
public sealed class GetCommunicationLogHandler(
    [FromKeyedServices("microfinance:communicationlogs")] IReadRepository<CommunicationLog> repository,
    ILogger<GetCommunicationLogHandler> logger)
    : IRequestHandler<GetCommunicationLogRequest, CommunicationLogResponse>
{
    public async Task<CommunicationLogResponse> Handle(GetCommunicationLogRequest request, CancellationToken cancellationToken)
    {
        var spec = new CommunicationLogByIdSpec(request.Id);
        var communicationLog = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (communicationLog is null)
        {
            throw new NotFoundException($"Communication log with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved communication log {CommunicationLogId}", request.Id);

        return communicationLog;
    }
}
