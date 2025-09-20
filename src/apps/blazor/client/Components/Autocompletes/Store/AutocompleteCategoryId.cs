namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

public class AutocompleteCategoryId : AutocompleteBase<CategoryResponse, IApiClient, DefaultIdType>
{
    [Parameter] public bool ShowAll { get; set; }

    protected override async Task<CategoryResponse?> GetItem(Guid id)
    {
        return _dictionary.TryGetValue(id, out var dto)
            ? dto
            : await ApiHelper.ExecuteCallGuardedAsync(() => Client.GetCategoryEndpointAsync(id), Snackbar).ConfigureAwait(false);
    }

    protected override async Task<IEnumerable<Guid>> SearchText(string? value, CancellationToken token)
    {
        var request = new SearchCategoriesCommand
        {
            PageSize = 10,
            AdvancedSearch = new Search
            {
                Fields = ["code", "name", "description", "notes"],
                Keyword = value
            }
        };

        if (await SearchEmployees(request)is { } response)
            _dictionary = response.Data.To_dictionary(x => x.Id);

        return _dictionary.Keys;
    }

    protected override string GetValueText(DefaultIdType id)
    {
        return _dictionary.TryGetValue(id, out var dto) ? dto.Name : string.Empty;
    }

    private async Task<CategoryResponsePagedList?> SearchEmployees(SearchCategoriesCommand filter)
    {
        return await ApiHelper.ExecuteCallGuardedAsync(() => Client.SearchCategoriesEndpointAsync(filter), Snackbar)
            .ConfigureAwait(false);
    }
}
