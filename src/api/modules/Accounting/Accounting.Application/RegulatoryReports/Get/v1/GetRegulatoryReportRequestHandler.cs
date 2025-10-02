using Accounting.Application.RegulatoryReports.Queries;
using Accounting.Application.RegulatoryReports.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.RegulatoryReports.Get.v1;

public sealed class GetRegulatoryReportRequestHandler(
    [FromKeyedServices("accounting:regulatoryreports")] IReadRepository<RegulatoryReport> repository)
    : IRequestHandler<GetRegulatoryReportRequest, RegulatoryReportResponse>
{
    public async Task<RegulatoryReportResponse> Handle(GetRegulatoryReportRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.FirstOrDefaultAsync(
            new RegulatoryReportByIdSpec(request.Id), cancellationToken);

        if (report == null)
        {
            throw new RegulatoryReportNotFoundException(request.Id);
        }

        return report.Adapt<RegulatoryReportResponse>();
    }
}
