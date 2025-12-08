using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Delete.v1;

public sealed class DeleteReportDefinitionHandler(
    ILogger<DeleteReportDefinitionHandler> logger,
    [FromKeyedServices("microfinance:reportdefinitions")] IRepository<ReportDefinition> repository)
    : IRequestHandler<DeleteReportDefinitionCommand>
{
    public async Task Handle(DeleteReportDefinitionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.FirstOrDefaultAsync(new ReportDefinitionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Report definition with id {request.Id} not found");

        await repository.DeleteAsync(report, cancellationToken);

        logger.LogInformation("Report definition {Id} deleted", request.Id);
    }
}

