using Accounting.Application.RegulatoryReports.Responses;

namespace Accounting.Application.RegulatoryReports.Get.v1;

public class GetRegulatoryReportRequest(DefaultIdType id) : IRequest<RegulatoryReportResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
