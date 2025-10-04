namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

public class AutocompleteUsoaCategory : MudAutocomplete<string>
{
    [Inject] protected IClient Client { get; set; } = default!;
    
    private List<ChartOfAccountResponse> _list = new();

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
            AdvancedSearch = new Search { Fields = new[] { "usoaCategory", "description", "notes" }, Keyword = value },
            PageSize = 10
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                    () => Client.ChartOfAccountSearchEndpointAsync("1", filter, cancellationToken))
                .ConfigureAwait(false)
            is var response)
        {
            _list = response?.Items?.ToList() ?? new List<ChartOfAccountResponse>();
        }

        return _list.Select(x => x.Name!);
    }
}
