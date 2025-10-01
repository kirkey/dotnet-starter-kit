namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

public class GetCycleCountSpecs : Specification<CycleCount, CycleCountResponse>
{
    public GetCycleCountSpecs(DefaultIdType id)
    {
        Query
            .Include(c => c.Items)
            .Where(c => c.Id == id);
    }
}
