namespace FSH.Starter.Blazor.Client.Components.Autocompletes.Hr;

/// <summary>
/// Autocomplete component for selecting an Employee.
/// Searches employees by name or code and returns the employee ID.
/// </summary>
public class AutocompleteEmployee : AutocompleteBase<EmployeeResponse, IClient, DefaultIdType?>
{
    protected override async Task<EmployeeResponse?> GetItem(DefaultIdType? id)
    {
        if (!id.HasValue) return null;
        
        try
        {
            var response = await Client.GetEmployeeEndpointAsync("1", id.Value).ConfigureAwait(false);
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
            var command = new SearchEmployeesRequest
            {
                PageNumber = 1,
                PageSize = 50,
                Keyword = value,
                OrderBy = ["FirstName", "LastName"]
            };

            var result = await Client.SearchEmployeesEndpointAsync("1", command, token).ConfigureAwait(false);
            
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
        
        if (_dictionary.TryGetValue(id, out var employee))
        {
            return $"{employee.FirstName} {employee.LastName} ({employee.EmployeeNumber})";
        }
        
        return id.ToString() ?? string.Empty;
    }
}

