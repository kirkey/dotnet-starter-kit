using FSH.Starter.Blazor.Infrastructure.Api;
namespace FSH.Starter.Blazor.Client.Pages.Hr.Employees.Educations;

public partial class EmployeeEducations
{
    protected EntityServerTableContext<EmployeeEducationResponse, DefaultIdType, EmployeeEducationViewModel> Context { get; set; } = null!;

    [SupplyParameterFromQuery]
    public string? EmployeeId { get; set; }

    public string FilterEmployeeId => EmployeeId ?? string.Empty;

    public string FilterSuffix => !string.IsNullOrEmpty(FilterEmployeeId) ? " (Filtered)" : string.Empty;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<EmployeeEducationResponse, DefaultIdType, EmployeeEducationViewModel>(
            entityName: "Employee Education",
            entityNamePlural: "Employee Education",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<EmployeeEducationResponse>(response => response.EducationLevel, "Education Level", "EducationLevel"),
                new EntityField<EmployeeEducationResponse>(response => response.FieldOfStudy, "Field of Study", "FieldOfStudy"),
                new EntityField<EmployeeEducationResponse>(response => response.Institution, "Institution", "Institution"),
                new EntityField<EmployeeEducationResponse>(response => response.Degree, "Degree", "Degree"),
                new EntityField<EmployeeEducationResponse>(response => response.GraduationDate, "Graduation Date", "GraduationDate", typeof(DateOnly)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchEmployeeEducationsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchEmployeeEducationsEndpointAsync("1", request);
                
                // Filter by EmployeeId if provided
                if (!string.IsNullOrEmpty(FilterEmployeeId) && DefaultIdType.TryParse(FilterEmployeeId, out var employeeGuid))
                {
                    result.Items = result.Items?.Where(e => e.EmployeeId == employeeGuid).ToList() ?? [];
                    result.TotalCount = result.Items.Count;
                }
                
                return result.Adapt<PaginationResponse<EmployeeEducationResponse>>();
            },
            createFunc: async education =>
            {
                await Client.CreateEmployeeEducationEndpointAsync("1", education.Adapt<CreateEmployeeEducationCommand>());
            },
            updateFunc: async (id, education) =>
            {
                await Client.UpdateEmployeeEducationEndpointAsync("1", id, education.Adapt<UpdateEmployeeEducationCommand>());
            },
            deleteFunc: async id =>
            {
                await Client.DeleteEmployeeEducationEndpointAsync("1", id);
            });

        await Task.CompletedTask;
    }

    private void ClearFilter()
    {
        NavigationManager.NavigateTo("/human-resources/employees/educations");
    }

    private async Task ShowEducationsHelp()
    {
        await DialogService.ShowAsync<EmployeeEducationsHelpDialog>("Employee Education Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

public class EmployeeEducationViewModel : UpdateEmployeeEducationCommand
{
    // Properties inherited from UpdateEmployeeEducationCommand
}
