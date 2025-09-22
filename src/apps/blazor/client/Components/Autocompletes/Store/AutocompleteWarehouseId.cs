namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Warehouse by its identifier.
/// - Fetches a single Warehouse by id when needed.
/// - Searches Categories by code/name/description/notes and caches results in-memory.
/// </summary>
public class AutocompleteWarehouseId : AutocompleteBase<WarehouseResponse, IClient, DefaultIdType>
{
    // Local cache for id -> dto lookups. We don't rely on base's private cache.
    private Dictionary<Guid, WarehouseResponse> _cache = [];

    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Gets a single Warehouse item by identifier.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <returns>The category response, or null if not found.</returns>
    protected override async Task<WarehouseResponse?> GetItem(DefaultIdType id)
    {
        if (_cache.TryGetValue(id, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.GetWarehouseAsync("1", id),
                Snackbar,
                Navigation)
            .ConfigureAwait(false);

        if (dto is not null) _cache[id] = dto;

        return dto;
    }

    /// <summary>
    /// returns the first page of categories.
    /// </summary>
    /// <param name="value">The search text.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Enumerable of category ids matching the search.</returns>
    protected override async Task<IEnumerable<DefaultIdType>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchWarehousesCommand()
        {
            PageSize = 10,
            AdvancedSearch = new Search
            {
                Fields = new[] { "name", "description", "notes" },
                Keyword = value
            }
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.SearchWarehousesAsync("1", request, token),
                Snackbar,
                Navigation)
            .ConfigureAwait(false);

        if (response?.Items is { } items)
        {
            // Overwrite cache with latest page of results; guard against null Ids.
            _cache = items
                .GroupBy(x => x.Id)
                .Select(g => g.First())
                .ToDictionary(x => x.Id);
        }

        return _cache.Keys.Cast<DefaultIdType>();
    }

    protected override string GetTextValue(DefaultIdType id)
    {
        return _cache.TryGetValue(id, out var dto) ? dto.Name ?? string.Empty : string.Empty;
    }
}
