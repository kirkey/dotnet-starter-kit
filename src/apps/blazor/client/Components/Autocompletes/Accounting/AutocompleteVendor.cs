namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Vendor.
/// Searches project costing entries and provides distinct vendor values from the results.
/// Ensures consistency in vendor naming for accurate spend analysis and reporting.
/// </summary>
public class AutocompleteVendor : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = default!;
    
    private HashSet<string> _vendors = [];

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
            var filter = new SearchProjectCostingsQuery
            {
                AdvancedSearch = new Search 
                { 
                    Fields = ["vendor"], 
                    Keyword = value 
                },
                PageNumber = 1,
                PageSize = 100
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                    () => Client.ProjectCostingSearchEndpointAsync("1", filter, token))
                .ConfigureAwait(false);

            if (response?.Items != null)
            {
                _vendors = response.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.Vendor))
                    .Select(x => x.Vendor!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _vendors.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}


