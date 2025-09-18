using Accounting.Application.Consumptions.Dtos;

namespace Accounting.Application.Consumptions.Queries;

public class SearchConsumptionDataQuery : IRequest<List<ConsumptionDataDto>>
{
    public DefaultIdType? MeterId { get; set; }
    public string? BillingPeriod { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

