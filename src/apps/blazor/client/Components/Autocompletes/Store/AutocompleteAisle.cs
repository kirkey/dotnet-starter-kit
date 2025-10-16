namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Warehouse Location Aisle.
/// Searches warehouse locations and provides distinct aisle values from the results.
/// Ensures consistency in aisle naming across locations for accurate warehouse organization.
/// </summary>
public class AutocompleteAisle : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = default!;
    
    private HashSet<string> _aisles = [];

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
    /// Searches warehouse locations and returns distinct aisle values.
    /// </summary>
    /// <param name="value">The search text.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Enumerable of distinct aisle values.</returns>
    private async Task<IEnumerable<string>> SearchText(string? value, CancellationToken token)
    {
        try
        {
            var filter = new SearchWarehouseLocationsCommand
            {
                Aisle = value,
                PageNumber = 1,
                PageSize = 100
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                    () => Client.SearchWarehouseLocationsEndpointAsync("1", filter, token))
                .ConfigureAwait(false);

            if (response?.Items != null)
            {
                _aisles = response.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.Aisle))
                    .Select(x => x.Aisle!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _aisles.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}
