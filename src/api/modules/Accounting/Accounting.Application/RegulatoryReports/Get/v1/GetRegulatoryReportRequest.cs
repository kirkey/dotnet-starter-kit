using Accounting.Application.RegulatoryReports.Dtos;

namespace Accounting.Application.RegulatoryReports.Get.v1;

public class GetRegulatoryReportRequest(DefaultIdType id) : IRequest<RegulatoryReportDto>
{
    public DefaultIdType Id { get; set; } = id;
}
