using Accounting.Application.FixedAssets.Dtos;
using Accounting.Domain;
using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;

namespace Accounting.Application.FixedAssets.Search;

public sealed class SearchFixedAssetsSpec : EntitiesByPaginationFilterSpec<FixedAsset, FixedAssetDto>
{
    public SearchFixedAssetsSpec(SearchFixedAssetsRequest request) : base(request)
    {
        Query
            .OrderBy(a => a.AssetName!, !request.HasOrderBy())
            .Where(a => a.AssetName!.Contains(request.AssetName!), !string.IsNullOrEmpty(request.AssetName))
            .Where(a => a.AssetType!.Contains(request.AssetType!), !string.IsNullOrEmpty(request.AssetType))
            .Where(a => a.Department!.Contains(request.Department!), !string.IsNullOrEmpty(request.Department))
            .Where(a => a.SerialNumber!.Contains(request.SerialNumber!), !string.IsNullOrEmpty(request.SerialNumber));
    }
}


