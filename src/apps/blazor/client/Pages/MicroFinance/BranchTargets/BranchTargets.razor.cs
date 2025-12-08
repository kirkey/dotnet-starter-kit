namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.BranchTargets;

/// <summary>
/// Branch Targets page logic. Manages performance targets for branches.
/// </summary>
public partial class BranchTargets
{
    protected EntityServerTableContext<BranchTargetSummaryResponse, DefaultIdType, BranchTargetViewModel> Context { get; set; } = null!;
    private EntityTable<BranchTargetSummaryResponse, DefaultIdType, BranchTargetViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();

    private string? _searchTargetType;
    private string? SearchTargetType
    {
        get => _searchTargetType;
        set
        {
            _searchTargetType = value;
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

        Context = new EntityServerTableContext<BranchTargetSummaryResponse, DefaultIdType, BranchTargetViewModel>(
            fields:
            [
                new EntityField<BranchTargetSummaryResponse>(dto => dto.TargetType, "Target Type", "TargetType"),
                new EntityField<BranchTargetSummaryResponse>(dto => dto.Period, "Period", "Period"),
                new EntityField<BranchTargetSummaryResponse>(dto => dto.TargetValue, "Target", "TargetValue", typeof(decimal)),
                new EntityField<BranchTargetSummaryResponse>(dto => dto.AchievedValue, "Achieved", "AchievedValue", typeof(decimal)),
                new EntityField<BranchTargetSummaryResponse>(dto => dto.AchievementPercentage, "Achievement %", "AchievementPercentage", typeof(decimal)),
                new EntityField<BranchTargetSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchBranchTargetsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchBranchTargetsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<BranchTargetSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateBranchTargetAsync("1", viewModel.Adapt<CreateBranchTargetCommand>()).ConfigureAwait(false);
            },
            entityName: "Branch Target",
            entityNamePlural: "Branch Targets",
            entityResource: FshResources.BranchTargets,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var target = await Client.GetBranchTargetAsync("1", id).ConfigureAwait(false);
        
        var parameters = new DialogParameters
        {
            { "Target", target }
        };

        await DialogService.ShowAsync<BranchTargetDetailsDialog>("Branch Target Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Show branch targets help dialog.
    /// </summary>
    private async Task ShowBranchTargetsHelp()
    {
        await DialogService.ShowAsync<BranchTargetsHelpDialog>("Branch Targets Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
