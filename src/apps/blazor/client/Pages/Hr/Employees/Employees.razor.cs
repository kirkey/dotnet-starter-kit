using FSH.Starter.Blazor.Infrastructure.Api;
namespace FSH.Starter.Blazor.Client.Pages.Hr.Employees;

public partial class Employees
{
    

    protected EntityServerTableContext<EmployeeResponse, DefaultIdType, EmployeeViewModel> Context { get; set; } = null!;

    private ClientPreference _preference = new();

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };
    private readonly DialogOptions _wizardDialogOptions = new() { CloseOnEscapeKey = false, MaxWidth = MaxWidth.Medium, FullWidth = true };

    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<EmployeeResponse, DefaultIdType, EmployeeViewModel>(
            entityName: "Employee",
            entityNamePlural: "Employees",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<EmployeeResponse>(response => response.FirstName, "First Name", "FirstName"),
                new EntityField<EmployeeResponse>(response => response.LastName, "Last Name", "LastName"),
                new EntityField<EmployeeResponse>(response => response.Email, "Email", "Email"),
                new EntityField<EmployeeResponse>(response => response.PhoneNumber, "Phone", "PhoneNumber"),
                new EntityField<EmployeeResponse>(response => response.EmploymentClassification, "Type", "EmploymentClassification"),
                new EntityField<EmployeeResponse>(response => response.HireDate, "Hire Date", "HireDate", typeof(DateOnly)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchEmployeesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchEmployeesEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<EmployeeResponse>>();
            },
            createFunc: async _ =>
            {
                await ShowEmployeeCreationWizard();
            },
            updateFunc: async (id, employee) =>
            {
                await Client.UpdateEmployeeEndpointAsync("1", id, employee.Adapt<UpdateEmployeeCommand>());
            },
            deleteFunc: async _ =>
            {
                Snackbar?.Add("Use termination to end employment", Severity.Info);
                await Task.CompletedTask;
            });
    }

    private async Task ShowEmployeesHelp()
    {
        await DialogService.ShowAsync<EmployeesHelpDialog>("Employees Help", new DialogParameters(), _helpDialogOptions);
    }

    private async Task ShowEmployeeCreationWizard()
    {
        await DialogService.ShowAsync<EmployeeCreationWizard>("Hire New Employee", new DialogParameters(), _wizardDialogOptions);
    }

    private void NavigateTo(string pageUrl, DefaultIdType employeeId)
    {
        NavigationManager.NavigateTo($"{pageUrl}?employeeId={employeeId}");
    }
}

public class EmployeeViewModel : UpdateEmployeeCommand
{
    // Properties inherited from UpdateEmployeeCommand
}
