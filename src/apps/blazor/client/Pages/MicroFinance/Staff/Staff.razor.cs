namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Staff;

/// <summary>
/// Staff page logic. Provides CRUD and search over Staff entities.
/// Manages MFI employees who deliver financial services.
/// </summary>
public partial class Staff
{
    protected EntityServerTableContext<StaffResponse, DefaultIdType, StaffViewModel> Context { get; set; } = null!;

    private EntityTable<StaffResponse, DefaultIdType, StaffViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchEmployeeNumber;
    private string? SearchEmployeeNumber
    {
        get => _searchEmployeeNumber;
        set
        {
            _searchEmployeeNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchName;
    private string? SearchName
    {
        get => _searchName;
        set
        {
            _searchName = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchRole;
    private string? SearchRole
    {
        get => _searchRole;
        set
        {
            _searchRole = value;
            _ = _table.ReloadDataAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<StaffResponse, DefaultIdType, StaffViewModel>(
            fields:
            [
                new EntityField<StaffResponse>(dto => dto.EmployeeNumber, "Emp #", "EmployeeNumber"),
                new EntityField<StaffResponse>(dto => dto.FullName, "Name", "FullName"),
                new EntityField<StaffResponse>(dto => dto.Email, "Email", "Email"),
                new EntityField<StaffResponse>(dto => dto.JobTitle, "Job Title", "JobTitle"),
                new EntityField<StaffResponse>(dto => dto.Role, "Role", "Role"),
                new EntityField<StaffResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<StaffResponse>(dto => dto.JoiningDate, "Joined", "JoiningDate", typeof(DateOnly)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchStaffCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchStaffAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<StaffResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateStaffAsync("1", viewModel.Adapt<CreateStaffCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateStaffAsync("1", id, viewModel.Adapt<UpdateStaffCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteStaffAsync("1", id).ConfigureAwait(false),
            entityName: "Staff",
            entityNamePlural: "Staff",
            entityResource: FshResources.Staff);
    }

    private async Task ShowStaffHelp()
    {
        await DialogService.ShowAsync<StaffHelpDialog>("Staff Management Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    private async Task ViewStaffDetails(DefaultIdType id)
    {
        var staff = await Client.GetStaffAsync("1", id).ConfigureAwait(false);
        if (staff != null)
        {
            var parameters = new DialogParameters { ["Staff"] = staff };
            await DialogService.ShowAsync<StaffDetailsDialog>("Staff Details", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
        }
    }
}
