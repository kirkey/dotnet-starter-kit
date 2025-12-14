namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Category by its identifier.
/// - Fetches a single Category by id when needed.
/// - Searches Categories by code/name/description/notes and caches results in-memory.
/// </summary>
public class AutocompleteParentAccountId : AutocompleteBase<ChartOfAccountResponse, IClient, DefaultIdType?>
{
    // Local cache for id -> dto lookups. We don't rely on base's private cache.
    private Dictionary<DefaultIdType, ChartOfAccountResponse> _cache = [];

    [Inject] protected NavigationManager Navigation { get; set; } = null!;

    /// <summary>
    /// Gets a single Category item by identifier.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <returns>The category response, or null if not found.</returns>
    protected override async Task<ChartOfAccountResponse?> GetItem(DefaultIdType? id)
    {
        if (!id.HasValue || id.Value == DefaultIdType.Empty) return null;
        if (_cache.TryGetValue(id.Value, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ChartOfAccountGetEndpointAsync("1", id.Value))
            .ConfigureAwait(false);

        if (dto is not null) _cache[id.Value] = dto;

        return dto;
    }

    /// <summary>
    /// returns the first page of categories.
    /// </summary>
    /// <param name="value">The search text.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Enumerable of category ids matching the search.</returns>
    protected override async Task<IEnumerable<DefaultIdType?>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchChartOfAccountRequest
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
                () => Client.ChartOfAccountSearchEndpointAsync("1", request, token))
            .ConfigureAwait(false);

        if (response?.Items is { } items)
        {
            // Overwrite cache with latest page of results; guard against null Ids.
            _cache = items
                .GroupBy(x => x.Id)
                .Select(g => g.First())
                .ToDictionary(x => x.Id);
        }

        return _cache.Keys.Cast<DefaultIdType?>();
    }

    protected override string GetTextValue(DefaultIdType? id)
    {
        if (!id.HasValue) return string.Empty;
        return _cache.TryGetValue(id.Value, out var dto) ? dto.Name ?? string.Empty : string.Empty;
    }
}
