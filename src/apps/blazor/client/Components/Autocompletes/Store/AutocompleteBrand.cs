namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Brand.
/// Searches items and provides distinct brand names from the results.
/// </summary>
public class AutocompleteBrand : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    
    private HashSet<string> _brands = [];

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
                Brand = value,
                OrderBy = ["Brand"]
            };

            var result = await Client.SearchItemsEndpointAsync("1", command, token).ConfigureAwait(false);
            
            if (result.Items != null)
            {
                _brands = result.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.Brand))
                    .Select(x => x.Brand!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _brands.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}
