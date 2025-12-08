using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Cancel.v1;

/// <summary>
/// Handler for canceling a report generation.
/// </summary>
public sealed class CancelReportGenerationHandler(
    ILogger<CancelReportGenerationHandler> logger,
    [FromKeyedServices("microfinance:reportgenerations")] IRepository<ReportGeneration> repository)
    : IRequestHandler<CancelReportGenerationCommand, CancelReportGenerationResponse>
{
    public async Task<CancelReportGenerationResponse> Handle(CancelReportGenerationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var reportGeneration = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (reportGeneration is null)
        {
            throw new FSH.Framework.Core.Exceptions.NotFoundException($"ReportGeneration with id {request.Id} not found.");
        }

        reportGeneration.Cancel(request.Reason);

        await repository.UpdateAsync(reportGeneration, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Report generation {Id} cancelled", request.Id);

        return new CancelReportGenerationResponse(reportGeneration.Id);
    }
}

