namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Chart Of Account by its AccountCode (nullable string).
/// - Fetches a single ChartOfAccount by code when needed (via search).
/// - Searches ChartOfAccounts by accountCode/name/description/notes and caches results in-memory.
/// - Returns AccountCode (nullable string) as value while showing the human-friendly Name (configurable).
/// </summary>
public class AutocompleteChartOfAccountCode : AutocompleteBase<ChartOfAccountResponse, IClient, string?>
{
    // Local cache for code -> dto lookups. We don't rely on base's private cache.
    private readonly Dictionary<string, ChartOfAccountResponse> _cache = new();

    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Controls how the selected text is displayed.
    /// Accepted values: "Name" (default), "Code", "CodeName", "NameCode".
    /// </summary>
    [Parameter]
    public string TextFormat { get; set; } = "Name";

    /// <summary>
    /// Gets a single ChartOfAccount item by AccountCode (string).
    /// </summary>
    protected override async Task<ChartOfAccountResponse?> GetItem(string? code)
    {
        if (string.IsNullOrWhiteSpace(code)) return null;
        if (_cache.TryGetValue(code, out var cached)) return cached;

        // There is no dedicated get-by-code endpoint; use search with AccountCode filter and take the first match.
        var request = new SearchChartOfAccountQuery
        {
            PageNumber = 1,
            PageSize = 1,
            AdvancedSearch = new Search
            {
                // Include accountCode explicitly to allow quick exact/starts-with lookups.
                Fields = new[] { "accountCode" },
                Keyword = code
            }
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ChartOfAccountSearchEndpointAsync("1", request))
            .ConfigureAwait(false);

        var dto = response?.Items?.FirstOrDefault();
        if (dto is not null && !string.IsNullOrWhiteSpace(dto.AccountCode))
        {
            _cache[dto.AccountCode] = dto;
        }

        return dto;
    }

    /// <summary>
    /// Returns the first page of chart of accounts matching the search text (by AccountCode/Name/Description/Notes) as a list of codes.
    /// </summary>
    protected override async Task<IEnumerable<string?>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchChartOfAccountQuery
        {
            PageNumber = 1,
            PageSize = 10,
            AdvancedSearch = new Search
            {
                Fields = new[] { "accountCode", "name", "description", "notes" },
                Keyword = value
            }
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ChartOfAccountSearchEndpointAsync("1", request, token))
            .ConfigureAwait(false);

        var items = response?.Items?.ToList() ?? new List<ChartOfAccountResponse>();

        // Refresh cache with returned items (keep only the latest results)
        _cache.Clear();
        foreach (var it in items)
        {
            if (it is { AccountCode: not null } && !string.IsNullOrWhiteSpace(it.AccountCode))
                _cache[it.AccountCode] = it;
        }

        return _cache.Keys; // keys are non-null strings, valid for IEnumerable<string?>
    }

    /// <summary>
    /// Display text for a code value.
    /// If available, uses the cached DTO and formats according to TextFormat.
    /// Fallbacks to the code itself when DTO is not cached.
    /// </summary>
    protected override string GetTextValue(string? code)
    {
        if (string.IsNullOrWhiteSpace(code)) return string.Empty;

        if (_cache.TryGetValue(code, out var dto))
        {
            var name = dto.Name ?? string.Empty;
            return TextFormat switch
            {
                "Code" => code,
                "CodeName" => string.IsNullOrWhiteSpace(name) ? code : $"{code} - {name}",
                "NameCode" => string.IsNullOrWhiteSpace(name) ? code : $"{name} ({code})",
                _ => name // default: Name
            };
        }

        // Fallback: we only have the code string
        return TextFormat is "Code" or "CodeName" or "NameCode" ? code : string.Empty;
    }
}
