namespace FSH.Starter.WebApi.Store.Application.Bins.Specs;

/// <summary>
/// Specification to get a bin by its ID.
/// </summary>
public sealed class GetBinByIdSpec : Specification<Bin>
{
    public GetBinByIdSpec(DefaultIdType binId)
    {
        Query.Where(bin => bin.Id == binId);
    }
}
