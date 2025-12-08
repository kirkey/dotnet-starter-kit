using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Deactivate.v1;

public sealed class DeactivateReportDefinitionHandler(
    ILogger<DeactivateReportDefinitionHandler> logger,
    [FromKeyedServices("microfinance:reportdefinitions")] IRepository<ReportDefinition> repository)
    : IRequestHandler<DeactivateReportDefinitionCommand, DeactivateReportDefinitionResponse>
{
    public async Task<DeactivateReportDefinitionResponse> Handle(DeactivateReportDefinitionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.FirstOrDefaultAsync(new ReportDefinitionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Report definition with id {request.Id} not found");

        report.Deactivate();
        await repository.UpdateAsync(report, cancellationToken);

        logger.LogInformation("Report definition {Id} deactivated", report.Id);
        return new DeactivateReportDefinitionResponse(report.Id, report.Status);
    }
}

