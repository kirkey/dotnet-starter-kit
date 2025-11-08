namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

public class AutocompleteAccountName : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = default!;
    
    private List<ChartOfAccountResponse> _list = [];

    [Parameter] public string Parent { get; set; } = default!;

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
        var filter = new SearchChartOfAccountQuery
        {
            AdvancedSearch = new Search { Fields = ["accountName", "name", "description", "notes"], Keyword = value },
            PageSize = 10
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
