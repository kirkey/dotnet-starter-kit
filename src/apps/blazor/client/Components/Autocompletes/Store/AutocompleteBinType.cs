namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Bin Type.
/// Searches bins and provides distinct bin type values from the results.
/// Common values include: Storage, Picking, Staging, Receiving, Shipping, Quarantine, Dock, Returns, etc.
/// </summary>
public class AutocompleteBinType : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = null!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = null!;
    
    private HashSet<string> _binTypes = [];

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
            var command = new SearchBinsCommand
            {
                PageNumber = 1,
                PageSize = 100,
                SearchTerm = value,
                OrderBy = ["LocationType"]
            };

            var result = await Client.SearchBinsEndpointAsync("1", command, token).ConfigureAwait(false);
            
            if (result.Items != null)
            {
                _binTypes = result.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.LocationType))
                    .Select(x => x.LocationType!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _binTypes.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}
