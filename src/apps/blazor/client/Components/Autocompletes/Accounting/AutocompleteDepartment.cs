namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Department.
/// Searches projects and provides distinct department values from the results.
/// Common values include: IT, Finance, HR, Operations, Sales, Marketing, R&D, etc.
/// </summary>
public class AutocompleteDepartment : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = null!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = null!;
    
    private HashSet<string> _departments = [];

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
            var filter = new SearchProjectsCommand
            {
                AdvancedSearch = new Search 
                { 
                    Fields = ["department"], 
                    Keyword = value 
                },
                PageNumber = 1,
                PageSize = 100
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                    () => Client.ProjectSearchEndpointAsync("1", filter, token))
                .ConfigureAwait(false);

            if (response?.Items != null)
            {
                _departments = response.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.Department))
                    .Select(x => x.Department!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _departments.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

