using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Activate.v1;

public sealed class ActivateReportDefinitionHandler(
    ILogger<ActivateReportDefinitionHandler> logger,
    [FromKeyedServices("microfinance:reportdefinitions")] IRepository<ReportDefinition> repository)
    : IRequestHandler<ActivateReportDefinitionCommand, ActivateReportDefinitionResponse>
{
    public async Task<ActivateReportDefinitionResponse> Handle(ActivateReportDefinitionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.FirstOrDefaultAsync(new ReportDefinitionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Report definition with id {request.Id} not found");

        report.Activate();
        await repository.UpdateAsync(report, cancellationToken);

        logger.LogInformation("Report definition {Id} activated", report.Id);
        return new ActivateReportDefinitionResponse(report.Id, report.Status);
    }
}
