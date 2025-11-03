namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Vendor by its identifier.
/// - Fetches a single Vendor by id when needed.
/// - Searches Vendors by vendorCode/name/email and caches results in-memory.
/// - Returns Ids (DefaultIdType?) as value while showing the human-friendly Name.
/// - Supports nullable values for optional vendor selection.
/// </summary>
public class AutocompleteVendorId : AutocompleteBase<VendorGetResponse, IClient, DefaultIdType?>
{
    // Local cache for id -> dto lookups
    private readonly Dictionary<DefaultIdType, VendorGetResponse> _cache = new();

    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Controls how the selected text is displayed.
    /// Accepted values: "Name" (default), "Code", "CodeName", "NameCode".
    /// </summary>
    [Parameter]
    public string TextFormat { get; set; } = "Name";
    
    /// <summary>
    /// Gets a single Vendor item by identifier.
    /// </summary>
    protected override async Task<VendorGetResponse?> GetItem(DefaultIdType? id)
    {
        if (!id.HasValue) return null;
        if (_cache.TryGetValue(id.Value, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.VendorGetEndpointAsync("1", id.Value))
            .ConfigureAwait(false);

        if (dto is not null) _cache[dto.Id] = dto;

        return dto;
    }

    /// <summary>
    /// Returns the first page of vendors matching the search text.
    /// </summary>
    protected override async Task<IEnumerable<DefaultIdType?>> SearchText(string? value, CancellationToken token)
    {
        var request = new VendorSearchQuery
        {
            PageNumber = 1,
            PageSize = 10,
            AdvancedSearch = new Search
            {
                Fields = new[] { "vendorCode", "name", "email", "terms" },
                Keyword = value
            }
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.VendorSearchEndpointAsync("1", request, token))
            .ConfigureAwait(false);

        var items = response?.Items?.ToList() ?? new List<VendorSearchResponse>();

        // Refresh cache with returned items
        _cache.Clear();
        foreach (var it in items)
        {
            // Map search response to get response for cache
            var vendorDto = new VendorGetResponse
            {
                Id = it.Id,
                VendorCode = it.VendorCode,
                Name = it.Name
            };
            _cache[it.Id] = vendorDto;
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
        if (!id.HasValue) return string.Empty;

        if (_cache.TryGetValue(id.Value, out var dto))
        {
            var name = dto.Name ?? string.Empty;
            var code = dto.VendorCode ?? string.Empty;
            
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

