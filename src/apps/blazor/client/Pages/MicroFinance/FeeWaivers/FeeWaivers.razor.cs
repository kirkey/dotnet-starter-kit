namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FeeWaivers;

/// <summary>
/// FeeWaivers page logic. Provides CRUD and search operations for FeeWaiver entities.
/// Manages fee waiver requests with approval workflow.
/// </summary>
public partial class FeeWaivers
{
    static FeeWaivers()
    {
        // Configure Mapster to convert DateTimeOffset? to DateTime? for FeeWaiverSummaryResponse -> FeeWaiverViewModel mapping
        TypeAdapterConfig<FeeWaiverSummaryResponse, FeeWaiverViewModel>.NewConfig()
            .Map(dest => dest.RequestDate, src => src.RequestDate.HasValue ? src.RequestDate.Value.DateTime : (DateTime?)null);
    }

    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<FeeWaiverSummaryResponse, DefaultIdType, FeeWaiverViewModel> Context { get; set; } = null!;

    private EntityTable<FeeWaiverSummaryResponse, DefaultIdType, FeeWaiverViewModel> _table = null!;

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

    private string? _searchWaiverType;
    private string? SearchWaiverType
    {
        get => _searchWaiverType;
        set
        {
            _searchWaiverType = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with fee waiver-specific configuration.
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

        Context = new EntityServerTableContext<FeeWaiverSummaryResponse, DefaultIdType, FeeWaiverViewModel>(
            fields:
            [
                new EntityField<FeeWaiverSummaryResponse>(dto => dto.Reference, "Reference", "Reference"),
                new EntityField<FeeWaiverSummaryResponse>(dto => dto.FeeChargeId, "Fee Charge", "FeeChargeId"),
                new EntityField<FeeWaiverSummaryResponse>(dto => dto.WaiverType, "Type", "WaiverType"),
                new EntityField<FeeWaiverSummaryResponse>(dto => dto.OriginalAmount, "Original", "OriginalAmount", typeof(decimal)),
                new EntityField<FeeWaiverSummaryResponse>(dto => dto.WaivedAmount, "Waived", "WaivedAmount", typeof(decimal)),
                new EntityField<FeeWaiverSummaryResponse>(dto => dto.RequestDate, "Request Date", "RequestDate", typeof(DateTimeOffset)),
                new EntityField<FeeWaiverSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchFeeWaiversCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchFeeWaiversAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<FeeWaiverSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateFeeWaiverAsync("1", viewModel.Adapt<CreateFeeWaiverCommand>()).ConfigureAwait(false);
            },
            entityName: "Fee Waiver",
            entityNamePlural: "Fee Waivers",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canApprove);

        // Check permissions
        var state = await AuthState;
        _canApprove = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show help dialog.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<FeeWaiversHelpDialog>("Fee Waivers Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View waiver details in a dialog.
    /// </summary>
    private async Task ViewDetails(DefaultIdType id)
    {
        var waiver = await Client.GetFeeWaiverAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters
        {
            { nameof(FeeWaiverDetailsDialog.Waiver), waiver }
        };

        await DialogService.ShowAsync<FeeWaiverDetailsDialog>("Waiver Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Approve a fee waiver.
    /// </summary>
    private async Task ApproveWaiver(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(FeeWaiverApproveDialog.WaiverId), id }
        };

        var dialog = await DialogService.ShowAsync<FeeWaiverApproveDialog>("Approve Waiver", parameters, new DialogOptions
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
    /// Reject a fee waiver.
    /// </summary>
    private async Task RejectWaiver(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(FeeWaiverRejectDialog.WaiverId), id }
        };

        var dialog = await DialogService.ShowAsync<FeeWaiverRejectDialog>("Reject Waiver", parameters, new DialogOptions
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
    /// Cancel a fee waiver.
    /// </summary>
    private async Task CancelWaiver(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(FeeWaiverCancelDialog.WaiverId), id }
        };

        var dialog = await DialogService.ShowAsync<FeeWaiverCancelDialog>("Cancel Waiver", parameters, new DialogOptions
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
