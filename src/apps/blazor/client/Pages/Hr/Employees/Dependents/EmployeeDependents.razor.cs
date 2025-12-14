namespace FSH.Starter.Blazor.Client.Pages.Hr.Employees.Dependents;

public partial class EmployeeDependents
{
    protected EntityServerTableContext<EmployeeDependentResponse, DefaultIdType, EmployeeDependentViewModel> Context { get; set; } = null!;

    [SupplyParameterFromQuery]
    public string? EmployeeId { get; set; }

    public string FilterEmployeeId => EmployeeId ?? string.Empty;

    public string FilterSuffix => !string.IsNullOrEmpty(FilterEmployeeId) ? " (Filtered)" : string.Empty;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<EmployeeDependentResponse, DefaultIdType, EmployeeDependentViewModel>(
            entityName: "Employee Dependent",
            entityNamePlural: "Employee Dependents",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<EmployeeDependentResponse>(response => response.FirstName, "First Name", "FirstName"),
                new EntityField<EmployeeDependentResponse>(response => response.LastName, "Last Name", "LastName"),
                new EntityField<EmployeeDependentResponse>(response => response.DependentType, "Type", "DependentType"),
                new EntityField<EmployeeDependentResponse>(response => response.DateOfBirth, "Birth Date", "DateOfBirth", typeof(DateOnly)),
                new EntityField<EmployeeDependentResponse>(response => response.Relationship, "Relationship", "Relationship"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchEmployeeDependentsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchEmployeeDependentsEndpointAsync("1", request);
                
                // Filter by EmployeeId if provided
                if (!string.IsNullOrEmpty(FilterEmployeeId) && DefaultIdType.TryParse(FilterEmployeeId, out var employeeGuid))
                {
                    result.Items = result.Items?.Where(d => d.EmployeeId == employeeGuid).ToList() ?? [];
                    result.TotalCount = result.Items.Count;
                }
                
                return result.Adapt<PaginationResponse<EmployeeDependentResponse>>();
            },
            createFunc: async dependent =>
            {
                await Client.CreateEmployeeDependentEndpointAsync("1", dependent.Adapt<CreateEmployeeDependentCommand>());
            },
            updateFunc: async (id, dependent) =>
            {
                await Client.UpdateEmployeeDependentEndpointAsync("1", id, dependent.Adapt<UpdateEmployeeDependentCommand>());
            },
            deleteFunc: async id =>
            {
                await Client.DeleteEmployeeDependentEndpointAsync("1", id);
            });

        await Task.CompletedTask;
    }

    private void ClearFilter()
    {
        NavigationManager.NavigateTo("/human-resources/employees/dependents");
    }

    private async Task ShowDependentsHelp()
    {
        await DialogService.ShowAsync<EmployeeDependentsHelpDialog>("Employee Dependents Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

public class EmployeeDependentViewModel : UpdateEmployeeDependentCommand
{
    // Properties inherited from UpdateEmployeeDependentCommand
}
