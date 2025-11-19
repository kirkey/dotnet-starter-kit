namespace FSH.Starter.Blazor.Client.Pages.Hr.Employees.Educations;

public partial class EmployeeEducations
{
    protected EntityServerTableContext<EmployeeEducationResponse, DefaultIdType, EmployeeEducationViewModel> Context { get; set; } = null!;

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
                new EntityField<EmployeeEducationResponse>(response => response.GraduationDate, "Graduation Date", "GraduationDate", typeof(DateTime)),
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
}

public class EmployeeEducationViewModel : UpdateEmployeeEducationCommand
{
    // Properties inherited from UpdateEmployeeEducationCommand
}

