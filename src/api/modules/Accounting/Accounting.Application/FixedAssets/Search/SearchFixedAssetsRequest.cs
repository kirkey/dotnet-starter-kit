using Accounting.Application.FixedAssets.Responses;

namespace Accounting.Application.FixedAssets.Search;

public class SearchFixedAssetsRequest : PaginationFilter, IRequest<PagedList<FixedAssetResponse>>
{
    public string? AssetName { get; set; }
    public string? AssetType { get; set; }
    public string? Department { get; set; }
    public string? SerialNumber { get; set; }
}


