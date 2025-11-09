namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Unit of Measure.
/// Searches items and provides distinct unit of measure values from the results.
/// Common values include: EA (Each), PCS (Pieces), BOX, PKG (Package), LB (Pound), KG (Kilogram), etc.
/// </summary>
public class AutocompleteUnitOfMeasure : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = null!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = null!;
    
    private HashSet<string> _unitsOfMeasure = [];

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
                OrderBy = ["UnitOfMeasure"]
            };

            var result = await Client.SearchItemsEndpointAsync("1", command, token).ConfigureAwait(false);
            
            if (result.Items != null)
            {
                _unitsOfMeasure = result.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.UnitOfMeasure))
                    .Select(x => x.UnitOfMeasure!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _unitsOfMeasure.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

