namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Bank by returning its Id (Guid).
/// - Fetches a single Bank by ID when needed.
/// - Searches Banks by bank code/name/routing number and caches results in-memory.
/// - Returns Bank Id (Guid) as value while showing the human-friendly display text (configurable).
/// </summary>
public class AutocompleteBank : AutocompleteBase<BankResponse, IClient, DefaultIdType?>
{
    // Local cache for id -> dto lookups.
    private readonly Dictionary<DefaultIdType, BankResponse> _cache = new();

    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Controls how the selected text is displayed.
    /// Accepted values: "Name" (default), "Code", "CodeName", "NameCode".
    /// </summary>
    [Parameter]
    public string TextFormat { get; set; } = "Name";

    /// <summary>
    /// Gets a single Bank item by Id (Guid).
    /// </summary>
    protected override async Task<BankResponse?> GetItem(DefaultIdType? id)
    {
        if (id == null || id == DefaultIdType.Empty) return null;
        if (_cache.TryGetValue(id.Value, out var cached)) return cached;

        try
        {
            var dto = await ApiHelper.ExecuteCallGuardedAsync(
                    () => Client.BankGetEndpointAsync("1", id.Value))
                .ConfigureAwait(false);

            if (dto is not null)
            {
                _cache[id.Value] = dto;
            }

            return dto;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the first page of banks matching the search text (by BankCode/Name/RoutingNumber) as a list of Ids.
    /// </summary>
    protected override async Task<IEnumerable<DefaultIdType?>> SearchText(string? value, CancellationToken token)
    {
        var request = new BankSearchCommand
        {
            PageNumber = 1,
            PageSize = 10,
            BankCode = value,
            Name = value,
            RoutingNumber = value,
            IsActive = true // Only show active banks
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.BankSearchEndpointAsync("1", request, token))
            .ConfigureAwait(false);

        var paginationResponse = response?.Adapt<PaginationResponse<BankResponse>>();
        var items = paginationResponse?.Items?.ToList() ?? [];

        // Refresh cache with returned items
        _cache.Clear();
        foreach (var it in items)
        {
            if (it.Id != DefaultIdType.Empty)
                _cache[it.Id] = it;
        }

        return _cache.Keys.Cast<DefaultIdType?>(); // Return Guids as nullable
    }

    /// <summary>
    /// Display text for a bank ID value.
    /// If available, uses the cached DTO and formats according to TextFormat.
    /// Fallbacks to empty string when DTO is not cached.
    /// </summary>
    protected override string GetTextValue(DefaultIdType? id)
    {
        if (id == null || id == DefaultIdType.Empty) return string.Empty;

        if (_cache.TryGetValue(id.Value, out var dto))
        {
            var name = dto.Name ?? string.Empty;
            var code = dto.BankCode ?? string.Empty;

            return TextFormat switch
            {
                "Code" => code,
                "CodeName" => string.IsNullOrWhiteSpace(name) ? code : $"{code} - {name}",
                "NameCode" => string.IsNullOrWhiteSpace(name) ? code : $"{name} ({code})",
                _ => name // default: Name
            };
        }

        // Fallback: we don't have the data cached
        return string.Empty;
    }
}

