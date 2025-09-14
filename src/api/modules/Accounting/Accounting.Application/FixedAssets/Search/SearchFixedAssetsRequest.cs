using Accounting.Application.FixedAssets.Dtos;

namespace Accounting.Application.FixedAssets.Search;

public class SearchFixedAssetsRequest : PaginationFilter, IRequest<PagedList<FixedAssetDto>>
{
    public string? AssetName { get; set; }
    public string? AssetType { get; set; }
    public string? Department { get; set; }
    public string? SerialNumber { get; set; }
}


