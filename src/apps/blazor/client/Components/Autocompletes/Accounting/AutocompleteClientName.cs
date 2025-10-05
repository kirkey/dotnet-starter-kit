namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Client Name.
/// Searches projects and provides distinct client name values from the results.
/// Ensures consistency in client naming across projects for accurate reporting and analytics.
/// </summary>
public class AutocompleteClientName : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = default!;
    
    private HashSet<string> _clientNames = [];

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
                    Fields = ["clientName"], 
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
                _clientNames = response.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.ClientName))
                    .Select(x => x.ClientName!)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToHashSet();
            }
            
            return _clientNames.OrderBy(x => x);
        }
        catch
        {
            return [];
        }
    }
}

