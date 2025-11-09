namespace FSH.Starter.Blazor.Client.Pages.Accounting.Projects;

/// <summary>
/// Projects page provides CRUD operations for Accounting Projects using the shared EntityTable pattern.
/// Mirrors the structure used in Budgets for consistency.
/// </summary>
public partial class Projects
{
    [Inject] protected ImageUrlService ImageUrlService { get; set; } = null!;

    protected EntityServerTableContext<ProjectResponse, DefaultIdType, ProjectViewModel> Context { get; set; } = null!;

    private EntityTable<ProjectResponse, DefaultIdType, ProjectViewModel> _table = null!;

    /// <summary>
    /// Configure the EntityTable context: fields, search, create, update and delete functions.
    /// </summary>
    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<ProjectResponse, DefaultIdType, ProjectViewModel>(
            entityName: "Project",
            entityNamePlural: "Projects",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<ProjectResponse>(response => response.ImageUrl, "Image", "ImageUrl", Template: TemplateImage),
                new EntityField<ProjectResponse>(r => r.Name, "Name", "Name"),
                new EntityField<ProjectResponse>(r => r.StartDate, "Start Date", "StartDate", typeof(DateOnly)),
                new EntityField<ProjectResponse>(r => r.EndDate, "End Date", "EndDate", typeof(DateOnly)),
                new EntityField<ProjectResponse>(r => r.BudgetedAmount, "Budgeted Amount", "BudgetedAmount", typeof(decimal)),
                new EntityField<ProjectResponse>(r => r.ClientName, "Client", "ClientName"),
                new EntityField<ProjectResponse>(r => r.ProjectManager, "Manager", "ProjectManager"),
                new EntityField<ProjectResponse>(r => r.Department, "Department", "Department"),
                new EntityField<ProjectResponse>(r => r.ActualCost, "Actual Cost", "ActualCost", typeof(decimal)),
                new EntityField<ProjectResponse>(r => r.ActualRevenue, "Actual Revenue", "ActualRevenue", typeof(decimal)),
                new EntityField<ProjectResponse>(r => r.Description, "Description", "Description"),
                new EntityField<ProjectResponse>(r => r.Notes, "Notes", "Notes"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = filter.Adapt<SearchProjectsCommand>();
                var result = await Client.ProjectSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<ProjectResponse>>();
            },
            createFunc: async vm =>
            {
                vm.Image = new FileUploadCommand
                {
                    Name = vm.Image?.Name,
                    Extension = vm.Image?.Extension,
                    Data = vm.Image?.Data,
                    Size = vm.Image?.Size,
                };
                await Client.ProjectCreateEndpointAsync("1", vm.Adapt<CreateProjectCommand>());
            },
            updateFunc: async (id, vm) =>
            {
                vm.Image = new FileUploadCommand
                {
                    Name = vm.Image?.Name,
                    Extension = vm.Image?.Extension,
                    Data = vm.Image?.Data,
                    Size = vm.Image?.Size,
                };
                await Client.ProjectUpdateEndpointAsync("1", id, vm.Adapt<UpdateProjectCommand>());
            },
            deleteFunc: async id => await Client.ProjectDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }
}
