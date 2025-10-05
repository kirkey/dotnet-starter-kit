namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Weight Unit.
/// Searches items and provides distinct weight unit values from the results.
/// Common values include: kg (Kilogram), lb (Pound), g (Gram), oz (Ounce), ton, etc.
/// </summary>
public class AutocompleteWeightUnit : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = default!;
    
    private HashSet<string> _weightUnits = [];

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
                SearchTerm = value,
                OrderBy = ["WeightUnit"]
            };

            var result = await Client.SearchItemsEndpointAsync("1", command, token).ConfigureAwait(false);
            
            if (result.Items != null)
            {
                _weightUnits = result.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.WeightUnit))
                    .Select(x => x.WeightUnit!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _weightUnits.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

