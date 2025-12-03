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
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchProjectName;
    private string? SearchProjectName
    {
        get => _searchProjectName;
        set
        {
            _searchProjectName = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchClientName;
    private string? SearchClientName
    {
        get => _searchClientName;
        set
        {
            _searchClientName = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Configure the EntityTable context: fields, search, create, update and delete functions.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
            _preference = preference;

        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

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
                new EntityField<ProjectResponse>(r => r.ActualCost, "Actual Cost", "ActualCost", typeof(decimal)),
                new EntityField<ProjectResponse>(r => r.ActualRevenue, "Actual Revenue", "ActualRevenue", typeof(decimal)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchProjectsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.ProjectSearchEndpointAsync("1", request).ConfigureAwait(false);
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
                await Client.ProjectCreateEndpointAsync("1", vm.Adapt<CreateProjectCommand>()).ConfigureAwait(false);
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
                await Client.ProjectUpdateEndpointAsync("1", id, vm.Adapt<UpdateProjectCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.ProjectDeleteEndpointAsync("1", id).ConfigureAwait(false));

        await Task.CompletedTask;
    }

    /// <summary>
    /// Show projects help dialog.
    /// </summary>
    private async Task ShowProjectsHelp()
    {
        await DialogService.ShowAsync<ProjectsHelpDialog>("Projects Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
