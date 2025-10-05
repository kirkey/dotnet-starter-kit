namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Project Manager.
/// Searches projects and provides distinct project manager values from the results.
/// Ensures consistent naming for project manager assignment and reporting.
/// </summary>
public class AutocompleteProjectManager : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = default!;
    
    private HashSet<string> _projectManagers = [];

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
                    Fields = ["projectManager"], 
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
                _projectManagers = response.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.ProjectManager))
                    .Select(x => x.ProjectManager!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _projectManagers.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

