using Accounting.Application.RegulatoryReports.Dtos;

namespace Accounting.Application.RegulatoryReports.Get.v1;

public class GetRegulatoryReportRequest : IRequest<RegulatoryReportDto>
{
    public DefaultIdType Id { get; set; }

    public GetRegulatoryReportRequest(DefaultIdType id)
    {
        Id = id;
    }
}
