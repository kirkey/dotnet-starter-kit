using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Queue.v1;

public sealed class QueueReportGenerationHandler(
    ILogger<QueueReportGenerationHandler> logger,
    [FromKeyedServices("microfinance:reportgenerations")] IRepository<ReportGeneration> repository)
    : IRequestHandler<QueueReportGenerationCommand, QueueReportGenerationResponse>
{
    public async Task<QueueReportGenerationResponse> Handle(QueueReportGenerationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var generation = ReportGeneration.Queue(
            request.ReportDefinitionId,
            request.Trigger,
            request.OutputFormat,
            request.RequestedByUserId,
            request.Parameters,
            request.StartDate,
            request.EndDate,
            request.BranchId);

        await repository.AddAsync(generation, cancellationToken);
        logger.LogInformation("Report generation queued with ID {Id}", generation.Id);

        return new QueueReportGenerationResponse(generation.Id, generation.Status);
    }
}
