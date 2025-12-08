using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Update.v1;

public sealed class UpdateReportDefinitionHandler(
    ILogger<UpdateReportDefinitionHandler> logger,
    [FromKeyedServices("microfinance:reportdefinitions")] IRepository<ReportDefinition> repository)
    : IRequestHandler<UpdateReportDefinitionCommand, UpdateReportDefinitionResponse>
{
    public async Task<UpdateReportDefinitionResponse> Handle(UpdateReportDefinitionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.FirstOrDefaultAsync(new ReportDefinitionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Report definition with id {request.Id} not found");

        report.Update(
            request.Name,
            request.Description,
            request.ParametersDefinition,
            request.Query,
            request.LayoutTemplate,
            request.OutputFormat,
            request.Notes);

        await repository.UpdateAsync(report, cancellationToken);

        logger.LogInformation("Report definition {Id} updated", report.Id);
        return new UpdateReportDefinitionResponse(report.Id);
    }
}

