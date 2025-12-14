namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Chart Of Account by its identifier.
/// - Fetches a single ChartOfAccount by id when needed.
/// - Searches ChartOfAccounts by accountCode/name/description/notes and caches results in-memory.
/// - Returns Ids (DefaultIdType?) as value while showing the human-friendly Name.
/// - Supports nullable values for optional account selection.
/// </summary>
public class AutocompleteChartOfAccountId : AutocompleteBase<ChartOfAccountResponse, IClient, DefaultIdType?>
{
    // Local cache for id -> dto lookups. We don't rely on base's private cache.
    private Dictionary<DefaultIdType, ChartOfAccountResponse> _cache = new();

    [Inject] protected NavigationManager Navigation { get; set; } = null!;

    /// <summary>
    /// Controls how the selected text is displayed.
    /// Accepted values: "Name" (default), "Code", "CodeName", "NameCode".
    /// </summary>
    [Parameter]
    public string TextFormat { get; set; } = "Name";
    
    /// <summary>
    /// Gets a single ChartOfAccount item by identifier.
    /// </summary>
    protected override async Task<ChartOfAccountResponse?> GetItem(DefaultIdType? id)
    {
        if (id is null || id.Value == DefaultIdType.Empty) return null;
        if (_cache.TryGetValue(id.Value, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ChartOfAccountGetEndpointAsync("1", id.Value))
            .ConfigureAwait(false);

        if (dto is not null && dto.Id != DefaultIdType.Empty) _cache[dto.Id] = dto;

        return dto;
    }

    /// <summary>
    /// Returns the first page of chart of accounts matching the search text (by Id list).
    /// </summary>
    protected override async Task<IEnumerable<DefaultIdType?>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchChartOfAccountRequest
        {
            PageNumber = 1,
            PageSize = 10,
            AdvancedSearch = new Search
            {
                Fields = ["accountCode", "name", "description", "notes"],
                Keyword = value
            }
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ChartOfAccountSearchEndpointAsync("1", request, token))
            .ConfigureAwait(false);

        var items = response?.Items?.ToList() ?? [];

        // Refresh cache with returned items (keep only the latest results)
        _cache.Clear();
        foreach (var it in items)
        {
            if (it != null && it.Id != DefaultIdType.Empty)
                _cache[it.Id] = it;
        }

        return _cache.Keys.Cast<DefaultIdType?>();
    }

    /// <summary>
    /// Display text for an Id value.
    /// If available, uses the cached DTO and formats according to TextFormat.
    /// Fallbacks to empty string when DTO is not cached.
    /// </summary>
    protected override string GetTextValue(DefaultIdType? id)
    {
        if (id is null or null) return string.Empty;

        if (_cache.TryGetValue(id.Value, out var dto))
        {
            var name = dto.Name ?? string.Empty;
            var code = dto.AccountCode ?? string.Empty;
            
            return TextFormat switch
            {
                "Code" => code,
                "CodeName" => string.IsNullOrWhiteSpace(name) ? code : $"{code} - {name}",
                "NameCode" => string.IsNullOrWhiteSpace(name) ? code : $"{name} ({code})",
                _ => name // default: Name
            };
        }

        // Fallback: DTO not cached
        return string.Empty;
    }
}
