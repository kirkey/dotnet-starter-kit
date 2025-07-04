using FSH.Starter.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Components.Autocompletes.App;

public class AutocompleteGroupByParent : MudAutocomplete<string>
{
    private List<GroupDto> _list = new();

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
        var filter = new GroupSearchCommand
        {
            AdvancedSearch = new Search { Fields = new[] { "code", "name", "description", "notes" }, Keyword = value },
            PageSize = 10
        };

        if (await ApiHelper.ExecuteCallGuardedAsync(
                    () => ApiClient.GroupSearchEndpointAsync("1", filter, cancellationToken), Toast, Navigation)
                .ConfigureAwait(false)
            is var response)
        {
            _list = response?.Items?.ToList() ?? new List<GroupDto>();
        }

        return _list.Where(x => x.Parent!.Equals(Parent, StringComparison.OrdinalIgnoreCase)).Select(x => x.Name)!;
    }
}
