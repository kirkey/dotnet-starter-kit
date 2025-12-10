namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Staff;

/// <summary>
/// Staff page logic. Provides CRUD and search over Staff entities using the generated API client.
/// Manages microfinance staff including tellers, loan officers, and branch administrators.
/// </summary>
public partial class Staff
{
    static Staff()
    {
        // Configure Mapster to convert DateTimeOffset to DateTime? for StaffResponse -> StaffViewModel mapping
        TypeAdapterConfig<StaffResponse, StaffViewModel>.NewConfig()
            .Map(dest => dest.JoiningDate, src => src.JoiningDate.DateTime);
    }

    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<StaffResponse, DefaultIdType, StaffViewModel> Context { get; set; } = null!;

    private EntityTable<StaffResponse, DefaultIdType, StaffViewModel> _table = null!;

    /// <summary>
    /// Authorization state for permission checks.
    /// </summary>
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    /// <summary>
    /// Authorization service for permission checks.
    /// </summary>
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    // Permission flags
    private bool _canManageStatus;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
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

    private string? _searchStatus;
    private string? SearchStatus
    {
        get => _searchStatus;
        set
        {
            _searchStatus = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchPosition;
    private string? SearchPosition
    {
        get => _searchPosition;
        set
        {
            _searchPosition = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with staff-specific configuration including fields, CRUD operations, and search functionality.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load initial preference from localStorage
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
                new EntityField<StaffResponse>(dto => dto.EmployeeNumber, "Employee #", "EmployeeNumber"),
                new EntityField<StaffResponse>(dto => dto.FirstName, "First Name", "FirstName"),
                new EntityField<StaffResponse>(dto => dto.LastName, "Last Name", "LastName"),
                new EntityField<StaffResponse>(dto => dto.Email, "Email", "Email"),
                new EntityField<StaffResponse>(dto => dto.Phone, "Phone", "Phone"),
                new EntityField<StaffResponse>(dto => dto.JobTitle, "Job Title", "JobTitle"),
                new EntityField<StaffResponse>(dto => dto.JoiningDate, "Joining Date", "JoiningDate", typeof(DateTimeOffset)),
                new EntityField<StaffResponse>(dto => dto.Status, "Status", "Status"),
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
            getDefaultsFunc: async () => await Task.FromResult(new StaffViewModel
            {
                DateJoined = DateTime.Today,
                Status = "Active"
            }),
            entityName: "Staff",
            entityNamePlural: "Staff",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canManageStatus);

        // Check permissions for extra actions
        var state = await AuthState;
        _canManageStatus = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show staff help dialog.
    /// </summary>
    private async Task ShowStaffHelp()
    {
        var parameters = new DialogParameters
        {
            { "ContentMarkdown", GetHelpContent() }
        };
        
        await DialogService.ShowMessageBox(
            "Staff Management Help",
            (MarkupString)GetHelpContent(),
            yesText: "Close");
    }

    private static string GetHelpContent() => """
        <h4>Staff Management</h4>
        <p>This page allows you to manage staff members across all branches.</p>
        
        <h5>Staff Status</h5>
        <ul>
            <li><strong>Active</strong> - Currently employed and working</li>
            <li><strong>On Leave</strong> - Temporarily away (vacation, sick leave, etc.)</li>
            <li><strong>Suspended</strong> - Temporarily suspended pending investigation</li>
            <li><strong>Terminated</strong> - Employment ended</li>
        </ul>
        
        <h5>Workflow Actions</h5>
        <ul>
            <li><strong>Suspend</strong> - Temporarily suspend an active staff member</li>
            <li><strong>Terminate</strong> - End employment for a staff member</li>
            <li><strong>Reinstate</strong> - Bring back a suspended or terminated staff member</li>
        </ul>
        """;

    /// <summary>
    /// Suspend a staff member.
    /// </summary>
    private async Task SuspendStaff(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Suspend Staff",
            "Are you sure you want to suspend this staff member? They will be temporarily unable to perform their duties.",
            yesText: "Suspend",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.SuspendStaffAsync("1", id, new SuspendStaffCommand { Id = id }),
                successMessage: "Staff member suspended successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Terminate a staff member.
    /// </summary>
    private async Task TerminateStaff(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Terminate Staff",
            "Are you sure you want to terminate this staff member? This will end their employment.",
            yesText: "Terminate",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.SuspendStaffAsync("1", id, new SuspendStaffCommand { Id = id }),
                successMessage: "Staff member suspended successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Reinstate a staff member.
    /// </summary>
    private async Task ReinstateStaff(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Reinstate Staff",
            "Are you sure you want to reinstate this staff member? They will be set back to Active status.",
            yesText: "Reinstate",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ReinstateStaffAsync("1", id),
                successMessage: "Staff member reinstated successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// View staff details in a dialog.
    /// </summary>
    private async Task ViewStaffDetails(DefaultIdType id)
    {
        var staff = await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.GetStaffAsync("1", id),
            successMessage: null);

        if (staff != null)
        {
            var parameters = new DialogParameters
            {
                { "ContentMarkdown", GetStaffDetailsContent(staff) }
            };

            await DialogService.ShowMessageBox(
                "Staff Details",
                (MarkupString)GetStaffDetailsContent(staff),
                yesText: "Close");
        }
    }

    private static string GetStaffDetailsContent(StaffResponse staff) => $"""
        <table style="width: 100%;">
            <tr><td><strong>Employee Number:</strong></td><td>{staff.EmployeeNumber}</td></tr>
            <tr><td><strong>Name:</strong></td><td>{staff.FirstName} {staff.LastName}</td></tr>
            <tr><td><strong>Email:</strong></td><td>{staff.Email}</td></tr>
            <tr><td><strong>Phone:</strong></td><td>{staff.Phone}</td></tr>
            <tr><td><strong>Job Title:</strong></td><td>{staff.JobTitle}</td></tr>
            <tr><td><strong>Branch ID:</strong></td><td>{staff.BranchId}</td></tr>
            <tr><td><strong>Joining Date:</strong></td><td>{staff.JoiningDate:yyyy-MM-dd}</td></tr>
            <tr><td><strong>Status:</strong></td><td>{staff.Status}</td></tr>
        </table>
        """;
}
