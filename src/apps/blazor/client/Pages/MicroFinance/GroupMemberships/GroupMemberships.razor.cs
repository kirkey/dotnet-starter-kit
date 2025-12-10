namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.GroupMemberships;

/// <summary>
/// GroupMemberships page logic. Provides CRUD and search operations for GroupMembership entities.
/// Manages the association between members and member groups.
/// </summary>
public partial class GroupMemberships
{
    static GroupMemberships()
    {
        // Configure Mapster to convert DateTimeOffset to DateTime? for GroupMembershipResponse -> GroupMembershipViewModel mapping
        TypeAdapterConfig<GroupMembershipResponse, GroupMembershipViewModel>.NewConfig()
            .Map(dest => dest.JoinDate, src => src.JoinDate.DateTime)
            .Map(dest => dest.LeftDate, src => src.LeaveDate.HasValue ? src.LeaveDate.Value.DateTime : (DateTime?)null);
    }

    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<GroupMembershipResponse, DefaultIdType, GroupMembershipViewModel> Context { get; set; } = null!;

    private EntityTable<GroupMembershipResponse, DefaultIdType, GroupMembershipViewModel> _table = null!;

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
    private bool _canManage;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
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

    /// <summary>
    /// Initializes the table context with group membership-specific configuration.
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

        Context = new EntityServerTableContext<GroupMembershipResponse, DefaultIdType, GroupMembershipViewModel>(
            fields:
            [
                new EntityField<GroupMembershipResponse>(dto => dto.GroupId, "Group ID", "GroupId"),
                new EntityField<GroupMembershipResponse>(dto => dto.MemberId, "Member ID", "MemberId"),
                new EntityField<GroupMembershipResponse>(dto => dto.Role, "Role", "Role"),
                new EntityField<GroupMembershipResponse>(dto => dto.JoinDate, "Joined", "JoinDate", typeof(DateTimeOffset)),
                new EntityField<GroupMembershipResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchGroupMembershipsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchGroupMembershipsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<GroupMembershipResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            getDefaultsFunc: async () => await Task.FromResult(new GroupMembershipViewModel
            {
                JoinedDate = DateTime.Today,
                Role = "Member"
            }),
            createFunc: async viewModel =>
            {
                await Client.CreateGroupMembershipAsync("1", viewModel.Adapt<CreateGroupMembershipCommand>()).ConfigureAwait(false);
            },
            entityName: "Group Membership",
            entityNamePlural: "Group Memberships",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canManage);

        // Check permissions
        var state = await AuthState;
        _canManage = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show help dialog.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<GroupMembershipsHelpDialog>("Group Memberships Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View membership details in a dialog.
    /// </summary>
    private async Task ViewMembershipDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(GroupMembershipDetailsDialog.MembershipId), id }
        };

        await DialogService.ShowAsync<GroupMembershipDetailsDialog>("Membership Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Deactivate a membership.
    /// </summary>
    private async Task DeactivateMembership(DefaultIdType id)
    {
        try
        {
            await Client.SuspendGroupMembershipAsync("1", id);
            await _table.ReloadDataAsync();
        }
        catch
        {
            // Handle error
        }
    }

    /// <summary>
    /// Reactivate a membership.
    /// </summary>
    private async Task ReactivateMembership(DefaultIdType id)
    {
        try
        {
            await Client.ReactivateGroupMembershipAsync("1", id);
            await _table.ReloadDataAsync();
        }
        catch
        {
            // Handle error
        }
    }
}
