namespace FSH.Starter.Blazor.Client.Components.Autocompletes;

/// <summary>
/// Autocomplete component for selecting warehouses.
/// </summary>
public class AutocompleteWarehouse : AutocompleteBase<WarehouseResponse, IClient, DefaultIdType>
{
    /// <summary>
    /// Gets a warehouse by ID.
    /// </summary>
    protected override async Task<WarehouseResponse?> GetItem(DefaultIdType id)
    {
        return await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.GetWarehouseEndpointAsync("1", id));
    }

    /// <summary>
    /// Searches warehouses by name or code.
    /// </summary>
    protected override async Task<IEnumerable<DefaultIdType>> SearchText(string? value, CancellationToken token)
    {
        var result = await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.SearchWarehousesEndpointAsync("1", new SearchWarehousesRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Keyword = value,
                IsActive = true
            }));

        if (result?.Items is null) return [];

        foreach (var item in result.Items)
        {
            _dictionary[item.Id] = item;
        }

        return result.Items.Select(x => x.Id);
    }

    /// <summary>
    /// Gets the display text for a warehouse.
    /// </summary>
    protected override string GetTextValue(DefaultIdType code)
    {
        return _dictionary.TryGetValue(code, out var item)
            ? $"{item.Code} - {item.Name}"
            : string.Empty;
    }
}

