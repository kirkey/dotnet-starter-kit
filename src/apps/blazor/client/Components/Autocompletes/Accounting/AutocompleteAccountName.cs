namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

public class AutocompleteAccountName : MudAutocomplete<string>
{
    private List<ChartOfAccountDto> _list = new();

    [Parameter] public string Parent { get; set; } = default!;

    [Inject] protected ISnackbar Snackbar { get; set; } = default!;
    [Inject] protected IApiClient ApiClient { get; set; } = default!;
    [Inject] protected ISnackbar Toast { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;

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

    private async Task<IEnumerable<string>>? SearchText(string? value, CancellationToken cancellationToken)
    {
        var filter = new SearchChartOfAccountRequest
        {
            AdvancedSearch = new Search { Fields = new[] { "accountName", "name", "description", "notes" }, Keyword = value },
            PageSize = 10
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                    () => ApiClient.ChartOfAccountSearchEndpointAsync("1", filter, cancellationToken), Toast, Navigation)
                .ConfigureAwait(false)
            is var response)
        {
            _list = response?.Items?.ToList() ?? new List<ChartOfAccountDto>();
        }

        return _list.Select(x => x.Name!);
    }
}
