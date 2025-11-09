namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting an Accounting Period by its identifier.
/// - Fetches a single AccountingPeriod by id when needed.
/// - Searches AccountingPeriods by name/description/notes and caches results in-memory.
/// - Returns nullable Ids (DefaultIdType?) as value while showing the human-friendly Name.
/// </summary>
public class AutocompleteAccountingPeriodId : AutocompleteBase<AccountingPeriodResponse, IClient, DefaultIdType?>
{
    // Local cache for id -> dto lookups. We don't rely on base's private cache.
    private Dictionary<DefaultIdType, AccountingPeriodResponse> _cache = new();

    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Gets a single AccountingPeriod item by identifier.
    /// </summary>
    protected override async Task<AccountingPeriodResponse?> GetItem(DefaultIdType? id)
    {
        if (!id.HasValue || id.Value == default) return null;
        if (_cache.TryGetValue(id.Value, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.AccountingPeriodGetEndpointAsync("1", id.Value))
            .ConfigureAwait(false);

        if (dto is not null && dto.Id != default) _cache[dto.Id] = dto;

        return dto;
    }

    /// <summary>
    /// Returns the first page of accounting periods matching the search text (by Id list).
    /// </summary>
    protected override async Task<IEnumerable<DefaultIdType?>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchAccountingPeriodsRequest
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
                () => Client.AccountingPeriodSearchEndpointAsync("1", request, token))
            .ConfigureAwait(false);

        var items = response?.Items?.ToList() ?? [];

        // Refresh cache with returned items (keep only the latest results)
        _cache.Clear();
        foreach (var it in items)
        {
            if (it != null && it.Id != default)
                _cache[it.Id] = it;
        }

        return _cache.Keys.Cast<DefaultIdType?>();
    }

    /// <summary>
    /// Display text for an Id value (shows the cached Name when available).
    /// </summary>
    protected override string GetTextValue(DefaultIdType? id)
    {
        return (id.HasValue && id.Value != default && _cache.TryGetValue(id.Value, out var dto)) ? dto.Name ?? string.Empty : string.Empty;
    }
}
