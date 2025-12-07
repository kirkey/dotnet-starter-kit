namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MemberGroups;

/// <summary>
/// MemberGroups page logic. Provides CRUD and search over MemberGroup entities.
/// Manages solidarity groups and village savings associations.
/// </summary>
public partial class MemberGroups
{
    protected EntityServerTableContext<MemberGroupResponse, DefaultIdType, MemberGroupViewModel> Context { get; set; } = null!;

    private EntityTable<MemberGroupResponse, DefaultIdType, MemberGroupViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private bool _canActivate;
    private bool _canDeactivate;
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchCode;
    private string? SearchCode
    {
        get => _searchCode;
        set
        {
            _searchCode = value;
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

    private string? _searchMeetingDay;
    private string? SearchMeetingDay
    {
        get => _searchMeetingDay;
        set
        {
            _searchMeetingDay = value;
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

        Context = new EntityServerTableContext<MemberGroupResponse, DefaultIdType, MemberGroupViewModel>(
            fields:
            [
                new EntityField<MemberGroupResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<MemberGroupResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<MemberGroupResponse>(dto => dto.LeaderName, "Leader", "LeaderName"),
                new EntityField<MemberGroupResponse>(dto => dto.MemberCount, "Members", "MemberCount", typeof(int)),
                new EntityField<MemberGroupResponse>(dto => dto.MeetingDay, "Meeting Day", "MeetingDay"),
                new EntityField<MemberGroupResponse>(dto => dto.MeetingTime, "Meeting Time", "MeetingTime", typeof(TimeOnly)),
                new EntityField<MemberGroupResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchMemberGroupsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchMemberGroupsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<MemberGroupResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateMemberGroupAsync("1", viewModel.Adapt<CreateMemberGroupCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateMemberGroupAsync("1", id, viewModel.Adapt<UpdateMemberGroupCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteMemberGroupAsync("1", id).ConfigureAwait(false),
            entityName: "Member Group",
            entityNamePlural: "Member Groups",
            entityResource: FshResources.MemberGroups,
            hasExtraActionsFunc: () => _canActivate || _canDeactivate);

        var state = await AuthState;
        _canActivate = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MemberGroups);
        _canDeactivate = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MemberGroups);
    }

    private async Task ShowMemberGroupsHelp()
    {
        await DialogService.ShowAsync<MemberGroupsHelpDialog>("Member Groups Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    private async Task ViewGroupDetails(DefaultIdType id)
    {
        var group = await Client.GetMemberGroupAsync("1", id).ConfigureAwait(false);
        if (group != null)
        {
            var parameters = new DialogParameters { ["Group"] = group };
            await DialogService.ShowAsync<MemberGroupDetailsDialog>("Member Group Details", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
        }
    }

    private async Task ManageMembers(DefaultIdType id)
    {
        var group = await Client.GetMemberGroupAsync("1", id).ConfigureAwait(false);
        if (group != null)
        {
            var parameters = new DialogParameters { ["Group"] = group };
            await DialogService.ShowAsync<GroupMembersDialog>("Group Members", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Large,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
        }
    }

    private async Task ActivateGroup(DefaultIdType id)
    {
        if (await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.ActivateMemberGroupAsync("1", id),
            Toast,
            successMessage: "Group activated successfully.") is not null)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task DeactivateGroup(DefaultIdType id)
    {
        if (await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.DeactivateMemberGroupAsync("1", id),
            Toast,
            successMessage: "Group deactivated successfully.") is not null)
        {
            await _table.ReloadDataAsync();
        }
    }
}
