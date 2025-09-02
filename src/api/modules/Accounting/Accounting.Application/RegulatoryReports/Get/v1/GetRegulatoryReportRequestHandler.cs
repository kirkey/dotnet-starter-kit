using Accounting.Application.RegulatoryReports.Dtos;
using Accounting.Application.RegulatoryReports.Exceptions;
using Accounting.Application.RegulatoryReports.Queries;
using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.RegulatoryReports.Get.v1;

public sealed class GetRegulatoryReportRequestHandler(
    [FromKeyedServices("accounting:regulatoryreports")] IReadRepository<RegulatoryReport> repository)
    : IRequestHandler<GetRegulatoryReportRequest, RegulatoryReportDto>
{
    public async Task<RegulatoryReportDto> Handle(GetRegulatoryReportRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.FirstOrDefaultAsync(
            new RegulatoryReportByIdSpec(request.Id), cancellationToken);

        if (report == null)
        {
            throw new RegulatoryReportNotFoundException(request.Id);
        }

        return report.Adapt<RegulatoryReportDto>();
    }
}
