namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Warehouse Location Section.
/// Searches warehouse locations and provides distinct section values from the results.
/// Ensures consistency in section naming across locations for accurate warehouse organization.
/// </summary>
public class AutocompleteSection : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = null!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = null!;
    
    private HashSet<string> _sections = [];

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
    /// Searches warehouse locations and returns distinct section values.
    /// </summary>
    /// <param name="value">The search text.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Enumerable of distinct section values.</returns>
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
                _sections = response.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.Section))
                    .Select(x => x.Section!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _sections.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

