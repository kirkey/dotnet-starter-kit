namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Warehouse by ID (non-nullable).
/// Provides search functionality and displays warehouse name.
/// </summary>
public class AutocompleteWarehouse : AutocompleteBase<WarehouseResponse, IClient, DefaultIdType>
{
    /// <summary>
    /// Gets a single Warehouse item by identifier.
    /// </summary>
    /// <param name="id">The warehouse identifier.</param>
    /// <returns>The warehouse response, or null if not found.</returns>
    protected override async Task<WarehouseResponse?> GetItem(DefaultIdType id)
    {
        if (_dictionary.TryGetValue(id, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.GetWarehouseEndpointAsync("1", id))
            .ConfigureAwait(false);

        if (dto is not null) _dictionary[id] = dto;

        return dto;
    }

    /// <summary>
    /// Returns the first page of warehouses.
    /// </summary>
    /// <param name="value">The search text.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Enumerable of warehouse ids matching the search.</returns>
    protected override async Task<IEnumerable<DefaultIdType>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchWarehousesRequest
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
                () => Client.SearchWarehousesEndpointAsync("1", request, token))
            .ConfigureAwait(false);

        if (response?.Items is { } items)
        {
            // Overwrite cache with latest page of results; guard against null Ids.
            _dictionary = items
                .Where(x => x.Id != Guid.Empty)
                .GroupBy(x => x.Id)
                .Select(g => g.First())
                .ToDictionary(x => x.Id);
        }

        return _dictionary.Keys;
    }

    protected override string GetTextValue(DefaultIdType id)
    {
        return _dictionary.TryGetValue(id, out var dto) ? dto.Name ?? string.Empty : string.Empty;
    }
}

