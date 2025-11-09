namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

public class AutocompleteUsoaCategory : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = null!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = null!;
    
    private List<ChartOfAccountResponse> _list = [];

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
            PageNumber = 1,
            PageSize = 10,
            AdvancedSearch = new Search
            {
                Fields = ["usoaCategory", "description", "notes"], Keyword = value
            },
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                    () => Client.ChartOfAccountSearchEndpointAsync("1", filter, cancellationToken))
                .ConfigureAwait(false)
            is var response)
        {
            _list = response?.Items?.ToList() ?? [];
        }

        return _list.Select(x => x.Name!);
    }
}
