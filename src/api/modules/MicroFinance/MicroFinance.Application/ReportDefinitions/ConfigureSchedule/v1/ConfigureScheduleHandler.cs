using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.ConfigureSchedule.v1;

public sealed class ConfigureScheduleHandler(
    ILogger<ConfigureScheduleHandler> logger,
    [FromKeyedServices("microfinance:reportdefinitions")] IRepository<ReportDefinition> repository)
    : IRequestHandler<ConfigureScheduleCommand, ConfigureScheduleResponse>
{
    public async Task<ConfigureScheduleResponse> Handle(ConfigureScheduleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.FirstOrDefaultAsync(new ReportDefinitionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Report definition with id {request.Id} not found");

        report.ConfigureSchedule(
            request.Frequency,
            request.Day,
            request.Time,
            request.Recipients);

        await repository.UpdateAsync(report, cancellationToken);

        logger.LogInformation("Report definition {Id} schedule configured with frequency {Frequency}", report.Id, request.Frequency);
        return new ConfigureScheduleResponse(report.Id, request.Frequency);
    }
}

