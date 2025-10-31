using Accounting.Application.RegulatoryReports.Queries;
using Accounting.Application.RegulatoryReports.Responses;

namespace Accounting.Application.RegulatoryReports.Get.v1;

public sealed class GetRegulatoryReportRequestHandler(
    [FromKeyedServices("accounting:regulatoryreports")] IReadRepository<RegulatoryReport> repository)
    : IRequestHandler<GetRegulatoryReportRequest, RegulatoryReportResponse>
{
    public async Task<RegulatoryReportResponse> Handle(GetRegulatoryReportRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new RegulatoryReportByIdSpec(request.Id);
        return await repository.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new RegulatoryReportNotFoundException(request.Id);
    }
}
