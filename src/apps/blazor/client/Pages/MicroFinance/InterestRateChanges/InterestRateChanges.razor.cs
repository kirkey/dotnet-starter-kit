namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InterestRateChanges;

/// <summary>
/// InterestRateChanges page logic. Provides CRUD and search operations for InterestRateChange entities.
/// Manages loan interest rate modifications with approval and application workflow.
/// </summary>
public partial class InterestRateChanges
{
    static InterestRateChanges()
    {
        // Configure Mapster to convert DateTimeOffset? to DateTime? for InterestRateChangeSummaryResponse -> InterestRateChangeViewModel mapping
        TypeAdapterConfig<InterestRateChangeSummaryResponse, InterestRateChangeViewModel>.NewConfig()
            .Map(dest => dest.RequestDate, src => src.RequestDate.HasValue ? src.RequestDate.Value.DateTime : (DateTime?)null)
            .Map(dest => dest.ApprovalDate, src => src.ApprovalDate.HasValue ? src.ApprovalDate.Value.DateTime : (DateTime?)null)
            .Map(dest => dest.EffectiveDate, src => src.EffectiveDate.HasValue ? src.EffectiveDate.Value.DateTime : (DateTime?)null);
    }

    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<InterestRateChangeSummaryResponse, DefaultIdType, InterestRateChangeViewModel> Context { get; set; } = null!;

    private EntityTable<InterestRateChangeSummaryResponse, DefaultIdType, InterestRateChangeViewModel> _table = null!;

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
    private bool _canApprove;
    private bool _canApply;

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

    private string? _searchChangeType;
    private string? SearchChangeType
    {
        get => _searchChangeType;
        set
        {
            _searchChangeType = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with interest rate change-specific configuration.
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

        Context = new EntityServerTableContext<InterestRateChangeSummaryResponse, DefaultIdType, InterestRateChangeViewModel>(
            fields:
            [
                new EntityField<InterestRateChangeSummaryResponse>(dto => dto.Reference, "Reference", "Reference"),
                new EntityField<InterestRateChangeSummaryResponse>(dto => dto.LoanId, "Loan", "LoanId"),
                new EntityField<InterestRateChangeSummaryResponse>(dto => dto.ChangeType, "Type", "ChangeType"),
                new EntityField<InterestRateChangeSummaryResponse>(dto => dto.PreviousRate, "Previous Rate", "PreviousRate", typeof(decimal)),
                new EntityField<InterestRateChangeSummaryResponse>(dto => dto.NewRate, "New Rate", "NewRate", typeof(decimal)),
                new EntityField<InterestRateChangeSummaryResponse>(dto => dto.EffectiveDate, "Effective Date", "EffectiveDate", typeof(DateTimeOffset)),
                new EntityField<InterestRateChangeSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchInterestRateChangesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchInterestRateChangesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<InterestRateChangeSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateInterestRateChangeAsync("1", viewModel.Adapt<CreateInterestRateChangeCommand>()).ConfigureAwait(false);
            },
            entityName: "Interest Rate Change",
            entityNamePlural: "Interest Rate Changes",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canApprove || _canApply);

        // Check permissions
        var state = await AuthState;
        _canApprove = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
        _canApply = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show help dialog.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<InterestRateChangesHelpDialog>("Interest Rate Changes Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View rate change details in a dialog.
    /// </summary>
    private async Task ViewDetails(DefaultIdType id)
    {
        var change = await Client.GetInterestRateChangeAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters
        {
            { nameof(InterestRateChangeDetailsDialog.RateChange), change }
        };

        await DialogService.ShowAsync<InterestRateChangeDetailsDialog>("Rate Change Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Approve an interest rate change.
    /// </summary>
    private async Task ApproveChange(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(InterestRateChangeApproveDialog.RateChangeId), id }
        };

        var dialog = await DialogService.ShowAsync<InterestRateChangeApproveDialog>("Approve Rate Change", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true
        });

        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Reject an interest rate change.
    /// </summary>
    private async Task RejectChange(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(InterestRateChangeRejectDialog.RateChangeId), id }
        };

        var dialog = await DialogService.ShowAsync<InterestRateChangeRejectDialog>("Reject Rate Change", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true
        });

        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Cancel an interest rate change.
    /// </summary>
    private async Task CancelChange(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(InterestRateChangeCancelDialog.RateChangeId), id }
        };

        var dialog = await DialogService.ShowAsync<InterestRateChangeCancelDialog>("Cancel Rate Change", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true
        });

        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Apply an approved interest rate change to the loan.
    /// </summary>
    private async Task ApplyChange(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(InterestRateChangeApplyDialog.RateChangeId), id }
        };

        var dialog = await DialogService.ShowAsync<InterestRateChangeApplyDialog>("Apply Rate Change", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true
        });

        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }
}
