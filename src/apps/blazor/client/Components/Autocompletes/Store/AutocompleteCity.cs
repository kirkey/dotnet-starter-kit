namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a City.
/// Searches suppliers and provides distinct city values from the results.
/// Common values include: New York, Los Angeles, Chicago, Toronto, London, etc.
/// </summary>
public class AutocompleteCity : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = default!;
    
    private HashSet<string> _cities = [];

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
                OrderBy = ["City"]
            };

            var result = await Client.SearchSuppliersEndpointAsync("1", command, token).ConfigureAwait(false);
            
            if (result.Items != null)
            {
                _cities = result.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.City))
                    .Select(x => x.City!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _cities.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

