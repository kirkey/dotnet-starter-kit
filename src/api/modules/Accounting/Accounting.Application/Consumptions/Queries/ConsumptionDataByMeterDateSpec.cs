namespace Accounting.Application.Consumptions.Queries;

public sealed class ConsumptionDataByMeterDateSpec : Specification<ConsumptionData>, ISingleResultSpecification<ConsumptionData>
{
    public ConsumptionDataByMeterDateSpec(DefaultIdType meterId, DateTime readingDate)
    {
        Query.Where(c => c.MeterId == meterId && c.ReadingDate == readingDate);
    }
}

