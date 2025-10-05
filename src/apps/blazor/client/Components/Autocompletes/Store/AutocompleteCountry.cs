namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Country.
/// Searches suppliers and provides distinct country values from the results.
/// Common values include: USA, Canada, Mexico, UK, Germany, China, Japan, etc.
/// </summary>
public class AutocompleteCountry : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = default!;
    
    private HashSet<string> _countries = [];

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
            var command = new SearchSuppliersCommand
            {
                PageNumber = 1,
                PageSize = 100,
                Keyword = value,
                OrderBy = ["Country"]
            };

            var result = await Client.SearchSuppliersEndpointAsync("1", command, token).ConfigureAwait(false);
            
            if (result.Items != null)
            {
                _countries = result.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.Country))
                    .Select(x => x.Country!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _countries.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

