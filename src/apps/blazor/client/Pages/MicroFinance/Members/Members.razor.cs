namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Members;

/// <summary>
/// Members page logic. Provides CRUD and search over Member entities using the generated API client.
/// Manages microfinance members including personal information, contact details, and financial data.
/// </summary>
public partial class Members
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<MemberResponse, DefaultIdType, MemberViewModel> Context { get; set; } = null!;

    private EntityTable<MemberResponse, DefaultIdType, MemberViewModel> _table = null!;

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
    private bool _canActivate;
    private bool _canDeactivate;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchMemberNumber;
    private string? SearchMemberNumber
    {
        get => _searchMemberNumber;
        set
        {
            _searchMemberNumber = value;
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

    private bool? _searchIsActive;
    private bool? SearchIsActive
    {
        get => _searchIsActive;
        set
        {
            _searchIsActive = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with member-specific configuration including fields, CRUD operations, and search functionality.
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

        Context = new EntityServerTableContext<MemberResponse, DefaultIdType, MemberViewModel>(
            fields:
            [
                new EntityField<MemberResponse>(dto => dto.MemberNumber, "Member #", "MemberNumber"),
                new EntityField<MemberResponse>(dto => dto.FullName, "Name", "FullName"),
                new EntityField<MemberResponse>(dto => dto.Email, "Email", "Email"),
                new EntityField<MemberResponse>(dto => dto.PhoneNumber, "Phone", "PhoneNumber"),
                new EntityField<MemberResponse>(dto => dto.City, "City", "City"),
                new EntityField<MemberResponse>(dto => dto.Occupation, "Occupation", "Occupation"),
                new EntityField<MemberResponse>(dto => dto.MonthlyIncome, "Monthly Income", "MonthlyIncome", typeof(decimal)),
                new EntityField<MemberResponse>(dto => dto.JoinDate, "Join Date", "JoinDate", typeof(DateOnly)),
                new EntityField<MemberResponse>(dto => dto.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchMembersCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchMembersAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<MemberResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateMemberAsync("1", viewModel.Adapt<CreateMemberCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateMemberAsync("1", id, viewModel.Adapt<UpdateMemberCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteMemberAsync("1", id).ConfigureAwait(false),
            entityName: "Member",
            entityNamePlural: "Members",
            entityResource: FshResources.Members,
            hasExtraActionsFunc: () => _canActivate || _canDeactivate);

        // Check permissions for extra actions
        var state = await AuthState;
        _canActivate = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.Members);
        _canDeactivate = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.Members);
    }

    /// <summary>
    /// Show members help dialog.
    /// </summary>
    private async Task ShowMembersHelp()
    {
        await DialogService.ShowAsync<MembersHelpDialog>("Members Management Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Activate a member.
    /// </summary>
    private async Task ActivateMember(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Activate Member",
            "Are you sure you want to activate this member?",
            yesText: "Activate",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ActivateMemberAsync("1", id),
                Snackbar,
                successMessage: "Member activated successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Deactivate a member.
    /// </summary>
    private async Task DeactivateMember(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Deactivate Member",
            "Are you sure you want to deactivate this member? They will not be able to access services.",
            yesText: "Deactivate",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.DeactivateMemberAsync("1", id),
                Snackbar,
                successMessage: "Member deactivated successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// View member details in a dialog.
    /// </summary>
    private async Task ViewMemberDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(MemberDetailsDialog.MemberId), id }
        };

        await DialogService.ShowAsync<MemberDetailsDialog>("Member Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
