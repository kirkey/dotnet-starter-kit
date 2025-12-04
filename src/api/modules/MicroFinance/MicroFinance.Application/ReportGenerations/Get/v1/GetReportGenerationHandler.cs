using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ReportGenerations.Get.v1;

public sealed class GetReportGenerationHandler(
    [FromKeyedServices("microfinance:reportgenerations")] IReadRepository<ReportGeneration> repository)
    : IRequestHandler<GetReportGenerationRequest, ReportGenerationResponse>
{
    public async Task<ReportGenerationResponse> Handle(GetReportGenerationRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var generation = await repository.FirstOrDefaultAsync(new ReportGenerationByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Report generation with id {request.Id} not found");

        return new ReportGenerationResponse(
            generation.Id,
            generation.ReportDefinitionId,
            generation.RequestedByUserId,
            generation.Trigger,
            generation.Parameters,
            generation.ReportStartDate,
            generation.ReportEndDate,
            generation.BranchId,
            generation.OutputFormat,
            generation.OutputFile,
            generation.FileSizeBytes,
            generation.RecordCount,
            generation.Status,
            generation.StartedAt,
            generation.CompletedAt,
            generation.DurationMs,
            generation.ErrorMessage,
            generation.CreatedOn);
    }
}
