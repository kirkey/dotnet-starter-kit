using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;

public sealed class GetReportDefinitionHandler(
    [FromKeyedServices("microfinance:reportdefinitions")] IReadRepository<ReportDefinition> repository)
    : IRequestHandler<GetReportDefinitionRequest, ReportDefinitionResponse>
{
    public async Task<ReportDefinitionResponse> Handle(GetReportDefinitionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var report = await repository.FirstOrDefaultAsync(new ReportDefinitionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Report definition with id {request.Id} not found");

        return new ReportDefinitionResponse(
            report.Id,
            report.Code,
            report.Name,
            report.Category,
            report.OutputFormat,
            report.Description,
            report.ParametersDefinition,
            report.IsScheduled,
            report.ScheduleFrequency,
            report.ScheduleDay,
            report.ScheduleTime,
            report.LastGeneratedAt,
            report.Status,
            report.CreatedOn);
    }
}
