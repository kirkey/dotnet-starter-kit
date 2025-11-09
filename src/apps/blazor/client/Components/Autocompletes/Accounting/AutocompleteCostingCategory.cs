namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Costing Category.
/// Searches project costing entries and provides distinct category values from the results.
/// Common values include: Labor, Materials, Equipment, Travel, Subcontractors, Overhead, etc.
/// </summary>
public class AutocompleteCostingCategory : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = null!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = null!;
    
    private HashSet<string> _categories = [];

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
                    Fields = ["category"], 
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
                _categories = response.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.Category))
                    .Select(x => x.Category!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _categories.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

