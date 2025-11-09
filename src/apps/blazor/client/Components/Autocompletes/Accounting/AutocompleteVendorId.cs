namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Accounting;

/// <summary>
/// Autocomplete component for selecting a Vendor by ID.
/// Searches vendor records and provides selection by vendor code and name.
/// </summary>
public class AutocompleteVendorId : MudAutocomplete<DefaultIdType?>
{
    [Inject] protected IClient Client { get; set; } = null!;
    [Inject] protected ApiHelper ApiHelper { get; set; } = null!;

    private List<VendorSearchResponse> _vendors = [];

    public override Task SetParametersAsync(ParameterView parameters)
    {
        Dense = true;
        ResetValueOnEmptyText = true;
        SearchFunc = SearchVendors;
        ToStringFunc = vendor => vendor.HasValue && _vendors.Any() 
            ? _vendors.FirstOrDefault(v => v.Id == vendor)?.Name ?? string.Empty
            : string.Empty;
        Clearable = true;
        Variant = Variant.Filled;
        return base.SetParametersAsync(parameters);
    }

    private async Task<IEnumerable<DefaultIdType?>> SearchVendors(string? value, CancellationToken token)
    {
        try
        {
            var filter = new VendorSearchRequest
            {
                Keyword = value,
                PageNumber = 1,
                PageSize = 10
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                    () => Client.VendorSearchEndpointAsync("1", filter, token))
                .ConfigureAwait(false);

            if (response?.Items != null)
            {
                _vendors = response.Items.ToList();
                return _vendors.Select(v => (DefaultIdType?)v.Id);
            }

            return [];
        }
        catch
        {
            return [];
        }
    }
}

