namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Branches;

/// <summary>
/// Branches page logic. Provides CRUD and search over Branch entities.
/// Manages MFI branch network including offices and service delivery points.
/// </summary>
public partial class Branches
{
    static Branches()
    {
        // Configure Mapster to convert DateTimeOffset? to DateTime? for BranchResponse -> BranchViewModel mapping
        TypeAdapterConfig<BranchResponse, BranchViewModel>.NewConfig()
            .Map(dest => dest.OpeningDate, src => src.OpeningDate.HasValue ? src.OpeningDate.Value.DateTime : (DateTime?)null)
            .Map(dest => dest.ClosingDate, src => src.ClosingDate.HasValue ? src.ClosingDate.Value.DateTime : (DateTime?)null);
    }

    /// <summary>
    /// Table context that drives the generic EntityTable used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<BranchResponse, DefaultIdType, BranchViewModel> Context { get; set; } = null!;

    private EntityTable<BranchResponse, DefaultIdType, BranchViewModel> _table = null!;

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

    private string? _searchBranchType;
    private string? SearchBranchType
    {
        get => _searchBranchType;
        set
        {
            _searchBranchType = value;
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

        Context = new EntityServerTableContext<BranchResponse, DefaultIdType, BranchViewModel>(
            fields:
            [
                new EntityField<BranchResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<BranchResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<BranchResponse>(dto => dto.BranchType, "Type", "BranchType"),
                new EntityField<BranchResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<BranchResponse>(dto => dto.Phone, "Phone", "Phone"),
                new EntityField<BranchResponse>(dto => dto.ManagerName, "Manager", "ManagerName"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchBranchesCommand
                {
                    Filter = new PaginationFilter
                    {
                        PageNumber = filter.PageNumber,
                        PageSize = filter.PageSize,
                        Keyword = filter.Keyword,
                        OrderBy = filter.OrderBy
                    }
                };
                var result = await Client.SearchBranchesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<BranchResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateBranchAsync("1", viewModel.Adapt<CreateBranchCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateBranchAsync("1", id, viewModel.Adapt<UpdateBranchCommand>()).ConfigureAwait(false);
            },
            entityName: "Branch",
            entityNamePlural: "Branches",
            entityResource: FshResources.Branches,
            hasExtraActionsFunc: () => _canActivate || _canDeactivate);

        var state = await AuthState;
        _canActivate = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.Branches);
        _canDeactivate = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.Branches);
    }

    private async Task ShowBranchesHelp()
    {
        await DialogService.ShowAsync<BranchesHelpDialog>("Branch Management Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    private async Task ViewBranchDetails(DefaultIdType id)
    {
        var branch = await Client.GetBranchAsync("1", id).ConfigureAwait(false);
        if (branch != null)
        {
            var parameters = new DialogParameters { ["Branch"] = branch };
            await DialogService.ShowAsync<BranchDetailsDialog>("Branch Details", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
        }
    }

    private async Task ActivateBranch(DefaultIdType id)
    {
        if (await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.ActivateBranchAsync("1", id),
            successMessage: "Branch activated successfully.") is not null)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task DeactivateBranch(DefaultIdType id)
    {
        if (await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.DeactivateBranchAsync("1", id),
            successMessage: "Branch deactivated successfully.") is not null)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Navigate to branch dashboard.
    /// </summary>
    private void OnViewDashboard(DefaultIdType id)
    {
        Navigation.NavigateTo($"/microfinance/branches/{id}/dashboard");
    }
}
