namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Hr;

/// <summary>
/// Autocomplete component for selecting an Organizational Unit (parent).
/// Searches organizational units and returns the unit ID.
/// </summary>
public class AutocompleteOrganizationalUnit : AutocompleteBase<OrganizationalUnitResponse, IClient, DefaultIdType?>
{
    protected override async Task<OrganizationalUnitResponse?> GetItem(DefaultIdType? id)
    {
        if (!id.HasValue || id.Value == DefaultIdType.Empty) return null;
        
        try
        {
            var response = await Client.GetOrganizationalUnitEndpointAsync("1", id.Value).ConfigureAwait(false);
            return response;
        }
        catch
        {
            return null;
        }
    }

    protected override async Task<IEnumerable<DefaultIdType?>> SearchText(string? value, CancellationToken token)
    {
        try
        {
            var command = new SearchOrganizationalUnitsRequest
            {
                PageNumber = 1,
                PageSize = 50,
                Keyword = value,
                OrderBy = ["Name"]
            };

            var result = await Client.SearchOrganizationalUnitsEndpointAsync("1", command, token).ConfigureAwait(false);
            
            if (result.Items != null)
            {
                foreach (var item in result.Items)
                {
                    _dictionary[item.Id] = item;
                }
                return result.Items.Select(x => (DefaultIdType?)x.Id);
            }
            
            return [];
        }
        catch
        {
            return [];
        }
    }

    protected override string GetTextValue(DefaultIdType? id)
    {
        if (!id.HasValue) return string.Empty;
        
        if (_dictionary.TryGetValue(id, out var unit))
        {
            return $"{unit.Name} ({unit.Code}) - {unit.Type}";
        }
        
        return id.ToString() ?? string.Empty;
    }
}

