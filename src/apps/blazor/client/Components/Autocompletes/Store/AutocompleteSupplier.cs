namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Supplier by its identifier.
/// - Fetches a single Supplier by id when needed.
/// - Searches Suppliers by name/code/email/phone and caches results in-memory.
/// </summary>
public class AutocompleteSupplier : AutocompleteBase<SupplierResponse, IClient, DefaultIdType?>
{
    // Local cache for id -> dto lookups
    private Dictionary<DefaultIdType, SupplierResponse> _cache = [];

    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Gets a single Supplier item by identifier.
    /// </summary>
    protected override async Task<SupplierResponse?> GetItem(DefaultIdType? id)
    {
        if (!id.HasValue) return null;
        if (_cache.TryGetValue(id.Value, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.GetSupplierEndpointAsync("1", id.Value),
                Snackbar,
                Navigation)
            .ConfigureAwait(false);

        if (dto is not null) _cache[id.Value] = dto;

        return dto;
    }

    /// <summary>
    /// Returns the first page of suppliers matching the search term.
    /// </summary>
    protected override async Task<IEnumerable<DefaultIdType?>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchSuppliersCommand
        {
            PageSize = 10,
            AdvancedSearch = new Search
            {
                Fields = new[] { "name", "code", "email", "phone" },
                Keyword = value
            }
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.SearchSuppliersEndpointAsync("1", request, token),
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

        return _cache.Keys.Cast<DefaultIdType?>();
    }

    protected override string GetTextValue(DefaultIdType? id)
    {
        if (!id.HasValue) return string.Empty;
        return _cache.TryGetValue(id.Value, out var dto) ? dto.Name ?? string.Empty : string.Empty;
    }
}
