namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Warehouse Location by ID (non-nullable).
/// Provides search functionality and displays warehouse location name.
/// </summary>
public class AutocompleteWarehouseLocation : AutocompleteBase<GetWarehouseLocationListResponse, IClient, DefaultIdType>
{
    // Local cache for id -> dto lookups.
    private Dictionary<DefaultIdType, GetWarehouseLocationListResponse> _cache = [];

    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] protected Snackbar Snackbar { get; set; } = default!;

    /// <summary>
    /// Gets a single Warehouse Location item by identifier.
    /// </summary>
    /// <param name="id">The warehouse location identifier.</param>
    /// <returns>The warehouse location response, or null if not found.</returns>
    protected override async Task<GetWarehouseLocationListResponse?> GetItem(DefaultIdType id)
    {
        if (_cache.TryGetValue(id, out var cached)) return cached;

        var dto = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.GetWarehouseLocationEndpointAsync("1", id))
            .ConfigureAwait(false);

        if (dto is not null)
        {
            // Convert GetWarehouseLocationResponse to GetWarehouseLocationListResponse
            var listResponse = new GetWarehouseLocationListResponse
            {
                Id = dto.Id,
                Name = dto.Name,
                Code = dto.Code,
                WarehouseId = dto.WarehouseId,
                WarehouseName = dto.WarehouseName,
                LocationType = dto.LocationType,
                IsActive = dto.IsActive
            };
            _cache[id] = listResponse;
            return listResponse;
        }

        return null;
    }

    /// <summary>
    /// Returns the first page of warehouse locations.
    /// </summary>
    /// <param name="value">The search text.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Enumerable of warehouse location ids matching the search.</returns>
    protected override async Task<IEnumerable<DefaultIdType>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchWarehouseLocationsCommand
        {
            PageNumber = 1,
            PageSize = 10,
            SearchTerm = value
        };

        var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.SearchWarehouseLocationsEndpointAsync("1", request, token))
            .ConfigureAwait(false);

        if (response?.Items is { } items)
        {
            // Overwrite cache with latest page of results; guard against null Ids.
            _cache = items
                .Where(x => x.Id != default)
                .GroupBy(x => x.Id)
                .Select(g => g.First())
                .ToDictionary(x => x.Id);
        }

        return _cache.Keys;
    }

    protected override string GetTextValue(DefaultIdType id)
    {
        return _cache.TryGetValue(id, out var dto) ? dto.Name ?? string.Empty : string.Empty;
    }
}
