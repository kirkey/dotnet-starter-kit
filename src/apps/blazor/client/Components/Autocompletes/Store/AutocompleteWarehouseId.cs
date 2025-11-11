namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Warehouse by its identifier.
/// - Fetches a single Warehouse by id when needed.
/// - Searches Categories by code/name/description/notes and caches results in-memory.
/// </summary>
public class AutocompleteWarehouseId : AutocompleteBase<WarehouseResponse, IClient, DefaultIdType?>
{
    // Local cache for id -> dto lookups. We don't rely on base's private cache.
    private Dictionary<DefaultIdType, WarehouseResponse> _cache = [];

    /// <summary>
    /// Gets a single Warehouse item by identifier.
    /// </summary>
    /// <param name="id">The warehouse identifier.</param>
    /// <returns>The warehouse response, or null if not found.</returns>
    protected override async Task<WarehouseResponse?> GetItem(DefaultIdType? id)
    {
        if (id is null or null) return null;
        
        if (_cache.TryGetValue(id.Value, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.GetWarehouseEndpointAsync("1", id.Value))
            .ConfigureAwait(false);

        if (dto is not null) _cache[id.Value] = dto;

        return dto;
    }

    /// <summary>
    /// Returns the first page of warehouses.
    /// </summary>
    /// <param name="value">The search text.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Enumerable of warehouse ids matching the search.</returns>
    protected override async Task<IEnumerable<DefaultIdType?>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchWarehousesRequest
        {
            PageNumber = 1,
            PageSize = 10,
            AdvancedSearch = new Search
            {
                Fields = ["name", "description", "notes"],
                Keyword = value
            }
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.SearchWarehousesEndpointAsync("1", request, token))
            .ConfigureAwait(false);

        if (response?.Items is { } items)
        {
            // Overwrite cache with latest page of results; guard against null Ids.
            _cache = items
                .Where(x => x.Id != DefaultIdType.Empty)
                .GroupBy(x => x.Id)
                .Select(g => g.First())
                .ToDictionary(x => x.Id);
        }

        return _cache.Keys.Cast<DefaultIdType?>();
    }

    protected override string GetTextValue(DefaultIdType? id)
    {
        if (id is null or null) return string.Empty;
        return _cache.TryGetValue(id.Value, out var dto) ? dto.Name ?? string.Empty : string.Empty;
    }
}
