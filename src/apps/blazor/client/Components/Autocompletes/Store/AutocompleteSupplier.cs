using FSH.Starter.Blazor.Client.Components.Autocompletes;

namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting a Supplier by ID.
/// Provides search functionality and displays supplier name and code.
/// </summary>
public class AutocompleteSupplier : AutocompleteBase<SupplierResponse, IClient, DefaultIdType>
{
    protected override async Task<SupplierResponse?> GetItem(DefaultIdType id)
    {
        try
        {
            return await Client.GetSupplierEndpointAsync("1", id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load supplier: {ex.Message}", Severity.Error);
            return null;
        }
    }

    protected override async Task<IEnumerable<DefaultIdType>> SearchText(string? value, CancellationToken token)
    {
        try
        {
            var command = new SearchSuppliersCommand
            {
                PageNumber = 1,
                PageSize = 10,
                Keyword = value,
                OrderBy = ["Name"]
            };

            var result = await Client.SearchSuppliersEndpointAsync("1", command, token).ConfigureAwait(false);
            
            foreach (var item in result.Items ?? [])
            {
                if (item.Id.HasValue)
                    _dictionary[item.Id.Value] = item;
            }
            
            return result.Items?.Where(x => x.Id.HasValue).Select(x => x.Id!.Value) ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to search suppliers: {ex.Message}", Severity.Error);
            return [];
        }
    }

    protected override string GetTextValue(DefaultIdType id)
    {
        if (!_dictionary.TryGetValue(id, out var supplier))
            return string.Empty;
        
        return $"{supplier.Name} ({supplier.Code})";
    }
}

