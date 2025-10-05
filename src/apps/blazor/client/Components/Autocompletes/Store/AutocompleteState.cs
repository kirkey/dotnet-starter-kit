namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a State/Province.
/// Searches suppliers and provides distinct state values from the results.
/// Common values include: California, Texas, New York, Ontario, Quebec, etc.
/// </summary>
public class AutocompleteState : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = default!;
    
    private HashSet<string> _states = [];

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
                OrderBy = ["State"]
            };

            var result = await Client.SearchSuppliersEndpointAsync("1", command, token).ConfigureAwait(false);
            
            if (result.Items != null)
            {
                _states = result.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.State))
                    .Select(x => x.State!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _states.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

