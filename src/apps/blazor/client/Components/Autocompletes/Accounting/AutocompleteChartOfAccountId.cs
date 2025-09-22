namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting an Accounting Period by its identifier.
/// - Fetches a single ChartOfAccount by id when needed.
/// - Searches ChartOfAccounts by name/description/notes and caches results in-memory.
/// - Returns Ids (DefaultIdType) as value while showing the human-friendly Name.
/// </summary>
public class AutocompleteChartOfAccountId : AutocompleteBase<ChartOfAccountResponse, IClient, DefaultIdType>
{
    // Local cache for id -> dto lookups. We don't rely on base's private cache.
    private Dictionary<DefaultIdType, ChartOfAccountResponse> _cache = new();

    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Gets a single ChartOfAccount item by identifier.
    /// </summary>
    protected override async Task<ChartOfAccountResponse?> GetItem(DefaultIdType id)
    {
        if (id == default) return null;
        if (_cache.TryGetValue(id, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ChartOfAccountGetEndpointAsync("1", id),
                Snackbar,
                Navigation)
            .ConfigureAwait(false);

        if (dto is not null && dto.Id != default) _cache[dto.Id] = dto;

        return dto;
    }

    /// <summary>
    /// Returns the first page of accounting periods matching the search text (by Id list).
    /// </summary>
    protected override async Task<IEnumerable<DefaultIdType>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchChartOfAccountQuery
        {
            PageNumber = 1,
            PageSize = 10,
            AdvancedSearch = new Search
            {
                Fields = new[] { "name", "description", "notes" },
                Keyword = value
            }
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ChartOfAccountSearchEndpointAsync("1", request, token),
                Snackbar,
                Navigation)
            .ConfigureAwait(false);

        var items = response?.Items?.ToList() ?? new List<ChartOfAccountResponse>();

        // Refresh cache with returned items (keep only the latest results)
        _cache.Clear();
        foreach (var it in items)
        {
            if (it != null && it.Id != default)
                _cache[it.Id] = it;
        }

        return _cache.Keys;
    }

    /// <summary>
    /// Display text for an Id value (shows the cached Name when available).
    /// </summary>
    protected override string GetTextValue(DefaultIdType id)
    {
        return (id != default && _cache.TryGetValue(id, out var dto)) ? dto.Name ?? string.Empty : string.Empty;
    }
}
