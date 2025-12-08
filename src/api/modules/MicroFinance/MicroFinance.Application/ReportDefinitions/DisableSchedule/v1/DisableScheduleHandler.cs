using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.DisableSchedule.v1;

public sealed class DisableScheduleHandler(
    ILogger<DisableScheduleHandler> logger,
    [FromKeyedServices("microfinance:reportdefinitions")] IRepository<ReportDefinition> repository)
    : IRequestHandler<DisableScheduleCommand, DisableScheduleResponse>
{
    public async Task<DisableScheduleResponse> Handle(DisableScheduleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.FirstOrDefaultAsync(new ReportDefinitionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Report definition with id {request.Id} not found");

        report.DisableSchedule();
        await repository.UpdateAsync(report, cancellationToken);

        logger.LogInformation("Report definition {Id} schedule disabled", report.Id);
        return new DisableScheduleResponse(report.Id);
    }
}

