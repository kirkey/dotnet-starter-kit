namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Store;

/// <summary>
/// Autocomplete component for selecting an Item by ID.
/// Provides search functionality and displays item name and SKU.
/// </summary>
public class AutocompleteItem : AutocompleteBase<ItemResponse, IClient, DefaultIdType>
{
    private readonly ISnackbar _snackbar = default!;
    
    protected override async Task<ItemResponse?> GetItem(DefaultIdType id)
    {
        try
        {
            return await Client.GetItemEndpointAsync("1", id).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Failed to load item: {ex.Message}", Severity.Error);
            return null;
        }
    }

    protected override async Task<IEnumerable<DefaultIdType>> SearchText(string? value, CancellationToken token)
    {
        try
        {
            var command = new SearchItemsCommand
            {
                PageNumber = 1,
                PageSize = 10,
                Keyword = value,
                OrderBy = ["Name"]
            };

            var result = await Client.SearchItemsEndpointAsync("1", command, token).ConfigureAwait(false);
            
            foreach (var item in result.Items ?? [])
            {
                _dictionary[item.Id] = item;
            }
            
            return result.Items?.Select(x => x.Id) ?? [];
        }
        catch (Exception ex)
        {
            _snackbar.Add($"Failed to search items: {ex.Message}", Severity.Error);
            return [];
        }
    }

    protected override string GetTextValue(DefaultIdType id)
    {
        if (!_dictionary.TryGetValue(id, out var item))
            return string.Empty;
        
        return $"{item.Name} ({item.Sku})";
    }
}

