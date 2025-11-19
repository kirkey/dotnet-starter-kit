namespace FSH.Starter.Blazor.Client.Pages.Hr.Employees.Contacts;

public partial class EmployeeContacts
{
    protected EntityServerTableContext<EmployeeContactResponse, DefaultIdType, EmployeeContactViewModel> Context { get; set; } = null!;

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
                return result.Adapt<PaginationResponse<EmployeeContactResponse>>();
            },
            createFunc: async contact =>
            {
                await Client.CreateEmployeeContactEndpointAsync("1", contact.Adapt<CreateEmployeeContactCommand>());
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
}

public class EmployeeContactViewModel : UpdateEmployeeContactCommand
{
    // Properties inherited from UpdateEmployeeContactCommand
}



