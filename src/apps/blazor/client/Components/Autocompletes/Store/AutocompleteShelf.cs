namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Warehouse Location Shelf.
/// Searches warehouse locations and provides distinct shelf values from the results.
/// Ensures consistency in shelf naming across locations for accurate warehouse organization.
/// </summary>
public class AutocompleteShelf : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = default!;
    
    private HashSet<string> _shelves = [];

    public override Task SetParametersAsync(ParameterView parameters)
    {
        CoerceText = true;
        CoerceValue = true;
        Clearable = true;
        Dense = true;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchText;
        Variant = Variant.Filled;
        return base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Searches warehouse locations and returns distinct shelf values.
    /// </summary>
    /// <param name="value">The search text.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Enumerable of distinct shelf values.</returns>
    private async Task<IEnumerable<string>> SearchText(string? value, CancellationToken token)
    {
        try
        {
            var filter = new SearchWarehouseLocationsCommand
            {
                SearchTerm = value,
                PageNumber = 1,
                PageSize = 100
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                    () => Client.SearchWarehouseLocationsEndpointAsync("1", filter, token))
                .ConfigureAwait(false);

            if (response?.Items != null)
            {
                _shelves = response.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.Shelf))
                    .Select(x => x.Shelf!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _shelves.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

