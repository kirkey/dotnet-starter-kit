namespace FSH.Starter.Blazor.Client.Pages.Hr.Employees.Contacts;

public partial class EmployeeContacts
{
    protected EntityServerTableContext<EmployeeContactResponse, DefaultIdType, EmployeeContactViewModel> Context { get; set; } = null!;

    [SupplyParameterFromQuery]
    public string? EmployeeId { get; set; }

    public string FilterEmployeeId => EmployeeId ?? string.Empty;

    public string FilterSuffix => !string.IsNullOrEmpty(FilterEmployeeId) ? " (Filtered)" : string.Empty;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<EmployeeContactResponse, DefaultIdType, EmployeeContactViewModel>(
            entityName: "Employee Contact",
            entityNamePlural: "Employee Contacts",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<EmployeeContactResponse>(response => response.FirstName, "First Name", "FirstName"),
                new EntityField<EmployeeContactResponse>(response => response.LastName, "Last Name", "LastName"),
                new EntityField<EmployeeContactResponse>(response => response.Relationship, "Relationship", "Relationship"),
                new EntityField<EmployeeContactResponse>(response => response.PhoneNumber, "Phone", "PhoneNumber"),
                new EntityField<EmployeeContactResponse>(response => response.Email, "Email", "Email"),
                new EntityField<EmployeeContactResponse>(response => response.Priority, "Priority", "Priority", typeof(int)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchEmployeeContactsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchEmployeeContactsEndpointAsync("1", request);
                
                // Filter by EmployeeId if provided
                if (!string.IsNullOrEmpty(FilterEmployeeId) && Guid.TryParse(FilterEmployeeId, out var employeeGuid))
                {
                    result.Items = result.Items?.Where(c => c.EmployeeId == employeeGuid).ToList() ?? [];
                    result.TotalCount = result.Items.Count;
                }
                
                return result.Adapt<PaginationResponse<EmployeeContactResponse>>();
            },
            createFunc: async contact =>
            {
                var command = contact.Adapt<CreateEmployeeContactCommand>();
                await Client.CreateEmployeeContactEndpointAsync("1", command);
            },
            updateFunc: async (id, contact) =>
            {
                await Client.UpdateEmployeeContactEndpointAsync("1", id, contact.Adapt<UpdateEmployeeContactCommand>());
            },
            deleteFunc: async id =>
            {
                await Client.DeleteEmployeeContactEndpointAsync("1", id);
            });

        await Task.CompletedTask;
    }

    private void ClearFilter()
    {
        NavigationManager.NavigateTo("/human-resources/employees/contacts");
    }
}

public class EmployeeContactViewModel : UpdateEmployeeContactCommand
{
    // Properties inherited from UpdateEmployeeContactCommand
}
