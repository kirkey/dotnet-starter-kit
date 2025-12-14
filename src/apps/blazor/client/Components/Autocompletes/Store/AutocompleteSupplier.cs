namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Supplier by ID.
/// Provides search functionality and displays supplier name and code.
/// </summary>
public class AutocompleteSupplier : AutocompleteBase<SupplierResponse, IClient, DefaultIdType?>
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    private Dictionary<DefaultIdType, SupplierResponse> _cache = [];

    protected override async Task<SupplierResponse?> GetItem(DefaultIdType? id)
    {
        if (!id.HasValue || id.Value == DefaultIdType.Empty) return null;
        if (_cache.TryGetValue(id.Value, out var cached)) return cached;

        try
        {
            var dto = await Client.GetSupplierEndpointAsync("1", id.Value).ConfigureAwait(false);
            if (dto is not null) _cache[id.Value] = dto;
            return dto;
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Failed to load supplier: {ex.Message}", Severity.Error);
            return null;
        }
    }

    protected override async Task<IEnumerable<DefaultIdType?>> SearchText(string? value, CancellationToken token)
    {
        try
        {
            var command = new SearchSuppliersCommand
            {
                PageNumber = 1,
                PageSize = 10,
                Keyword = value,
                OrderBy = ["Name"]
            };

            var result = await Client.SearchSuppliersEndpointAsync("1", command, token).ConfigureAwait(false);
            
            if (result?.Items is { } items)
            {
                _cache = items
                    .Where(x => x.Id.HasValue)
                    .GroupBy(x => x.Id!.Value)
                    .Select(g => g.First())
                    .ToDictionary(x => x.Id!.Value);
            }
            
            return result?.Items?.Where(x => x.Id.HasValue).Select(x => x.Id) ?? [];
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Failed to search suppliers: {ex.Message}", Severity.Error);
            return [];
        }
    }

    protected override string GetTextValue(DefaultIdType? id)
    {
        if (!id.HasValue || !_cache.TryGetValue(id.Value, out var supplier))
            return string.Empty;
        
        return $"{supplier.Name} ({supplier.Code})";
    }
}

