namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Stock Adjustment Reason.
/// Searches stock adjustments and provides distinct reason values from the results.
/// Common values include: Damaged, Expired, Lost, Found, Count Correction, Quality Issue, Theft, Breakage, etc.
/// </summary>
public class AutocompleteAdjustmentReason : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = null!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = null!;
    
    private HashSet<string> _reasons = [];

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
            var command = new SearchStockAdjustmentsCommand
            {
                PageNumber = 1,
                PageSize = 100,
                Reason = value,
                OrderBy = ["Reason"]
            };

            var result = await Client.SearchStockAdjustmentsEndpointAsync("1", command, token).ConfigureAwait(false);
            
            if (result.Items != null)
            {
                _reasons = result.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.Reason))
                    .Select(x => x.Reason!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _reasons.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}


