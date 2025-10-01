namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a GroceryItem by its identifier.
/// - Fetches a single GroceryItem by id when needed.
/// - Searches GroceryItems by name/SKU/barcode and caches results in-memory.
/// </summary>
public class AutocompleteGroceryItem : AutocompleteBase<GroceryItemResponse, IClient, DefaultIdType>
{
    // Local cache for id -> dto lookups
    private Dictionary<DefaultIdType, GroceryItemResponse> _cache = [];

    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Gets a single GroceryItem by identifier.
    /// </summary>
    protected override async Task<GroceryItemResponse?> GetItem(DefaultIdType id)
    {
        if (_cache.TryGetValue(id, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.GetGroceryItemEndpointAsync("1", id),
                Snackbar,
                Navigation)
            .ConfigureAwait(false);

        if (dto is not null) _cache[id] = dto;

        return dto;
    }

    /// <summary>
    /// Returns the first page of grocery items matching the search term.
    /// </summary>
    protected override async Task<IEnumerable<DefaultIdType>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchGroceryItemsQuery
        {
            PageSize = 10,
            AdvancedSearch = new Search
            {
                Fields = new[] { "name", "sku", "barcode" },
                Keyword = value
            }
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.SearchGroceryItemsEndpointAsync("1", request, token),
                Snackbar,
                Navigation)
            .ConfigureAwait(false);

        if (response?.Items is { } items)
        {
            // Overwrite cache with latest page of results; guard against null Ids
            _cache = items
                .Where(x => x.Id.HasValue)
                .GroupBy(x => x.Id!.Value)
                .Select(g => g.First())
                .ToDictionary(x => x.Id!.Value);
        }

        return _cache.Keys.Cast<DefaultIdType>();
    }

    protected override string GetTextValue(DefaultIdType id)
    {
        return _cache.TryGetValue(id, out var dto) 
            ? $"{dto.Name} ({dto.Sku})" 
            : string.Empty;
    }
}
