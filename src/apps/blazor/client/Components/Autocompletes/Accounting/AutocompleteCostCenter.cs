namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Cost Center.
/// Searches project costing entries and provides distinct cost center values from the results.
/// Common values include: HQ, Branch-East, Branch-West, Manufacturing, Admin, etc.
/// </summary>
public class AutocompleteCostCenter : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = null!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = null!;
    
    private HashSet<string> _costCenters = [];

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
                    Fields = ["costCenter"], 
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
                _costCenters = response.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.CostCenter))
                    .Select(x => x.CostCenter!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _costCenters.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

