using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportDefinitions.Create.v1;

public sealed class CreateReportDefinitionHandler(
    ILogger<CreateReportDefinitionHandler> logger,
    [FromKeyedServices("microfinance:reportdefinitions")] IRepository<ReportDefinition> repository)
    : IRequestHandler<CreateReportDefinitionCommand, CreateReportDefinitionResponse>
{
    public async Task<CreateReportDefinitionResponse> Handle(CreateReportDefinitionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = ReportDefinition.Create(
            request.Code,
            request.Name,
            request.Category,
            request.OutputFormat,
            request.Description,
            request.ParametersDefinition,
            request.Query,
            request.LayoutTemplate);

        await repository.AddAsync(report, cancellationToken);
        logger.LogInformation("Report definition {Code} created with ID {Id}", report.Code, report.Id);

        return new CreateReportDefinitionResponse(report.Id, report.Code);
    }
}
