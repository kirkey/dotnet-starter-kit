namespace FSH.Starter.Blazor.Client.Pages.Hr.Designations;

/// <summary>
/// Designations page for managing job titles and salary ranges.
/// Provides CRUD operations with area-specific salary configurations.
/// </summary>
public partial class Designations
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    protected EntityServerTableContext<DesignationResponse, DefaultIdType, DesignationViewModel> Context { get; set; } = null!;
    
    private ClientPreference _preference = new();

    private EntityTable<DesignationResponse, DefaultIdType, DesignationViewModel> _table = null!;

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
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

        Context = new EntityServerTableContext<DesignationResponse, DefaultIdType, DesignationViewModel>(
            entityName: "Designation",
            entityNamePlural: "Designations",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<DesignationResponse>(response => response.Code, "Code", "Code"),
                new EntityField<DesignationResponse>(response => response.Title, "Title", "Title"),
                new EntityField<DesignationResponse>(response => response.Area, "Area", "Area"),
                new EntityField<DesignationResponse>(response => response.SalaryGrade, "Grade", "SalaryGrade"),
                new EntityField<DesignationResponse>(response => response.MinimumSalary, "Min Salary", "MinimumSalary", typeof(decimal)),
                new EntityField<DesignationResponse>(response => response.MaximumSalary, "Max Salary", "MaximumSalary", typeof(decimal)),
                new EntityField<DesignationResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<DesignationResponse>(response => response.IsManagerial, "Managerial", "IsManagerial", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchDesignationsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchDesignationsEndpointAsync("1", request).ConfigureAwait(false);

                return result.Adapt<PaginationResponse<DesignationResponse>>();
            },
            createFunc: async designation =>
            {
                await Client.CreateDesignationEndpointAsync("1", designation.Adapt<CreateDesignationCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, designation) =>
            {
                await Client.UpdateDesignationEndpointAsync("1", id, designation.Adapt<UpdateDesignationCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteDesignationEndpointAsync("1", id).ConfigureAwait(false);
            });

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Shows the designations help dialog.
    /// </summary>
    private async Task ShowDesignationsHelp()
    {
        await DialogService.ShowAsync<DesignationsHelpDialog>("Designations Help", new DialogParameters(), _helpDialogOptions);
    }
}

