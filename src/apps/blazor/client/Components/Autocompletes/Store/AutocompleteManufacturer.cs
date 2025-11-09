namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Manufacturer.
/// Searches items and provides distinct manufacturer names from the results.
/// </summary>
public class AutocompleteManufacturer : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = null!;
    
    private HashSet<string> _manufacturers = [];

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

    private async Task<IEnumerable<string>> SearchText(string? value, CancellationToken token)
    {
        try
        {
            var command = new SearchItemsCommand
            {
                PageNumber = 1,
                PageSize = 100,
                Manufacturer = value,
                OrderBy = ["Manufacturer"]
            };

            var result = await Client.SearchItemsEndpointAsync("1", command, token).ConfigureAwait(false);
            
            if (result.Items != null)
            {
                _manufacturers = result.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.Manufacturer))
                    .Select(x => x.Manufacturer!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _manufacturers.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

