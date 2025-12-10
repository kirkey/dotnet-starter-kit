namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FeeCharges;

/// <summary>
/// FeeCharges page logic. Provides CRUD and search operations for FeeCharge entities.
/// Manages fees charged to member accounts with payment tracking.
/// </summary>
public partial class FeeCharges
{
    static FeeCharges()
    {
        // Configure Mapster to convert DateTimeOffset to DateTime? for FeeChargeResponse -> FeeChargeViewModel mapping
        TypeAdapterConfig<FeeChargeResponse, FeeChargeViewModel>.NewConfig()
            .Map(dest => dest.ChargeDate, src => src.ChargeDate.DateTime)
            .Map(dest => dest.DueDate, src => src.DueDate.HasValue ? src.DueDate.Value.DateTime : (DateTime?)null)
            .Map(dest => dest.PaidDate, src => src.PaidDate.HasValue ? src.PaidDate.Value.DateTime : (DateTime?)null);
    }

    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<FeeChargeResponse, DefaultIdType, FeeChargeViewModel> Context { get; set; } = null!;

    private EntityTable<FeeChargeResponse, DefaultIdType, FeeChargeViewModel> _table = null!;

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

    /// <summary>
    /// Initializes the table context with fee charge-specific configuration.
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

        Context = new EntityServerTableContext<FeeChargeResponse, DefaultIdType, FeeChargeViewModel>(
            fields:
            [
                new EntityField<FeeChargeResponse>(dto => dto.Reference, "Reference", "Reference"),
                new EntityField<FeeChargeResponse>(dto => dto.FeeDefinitionId, "Fee Definition", "FeeDefinitionId"),
                new EntityField<FeeChargeResponse>(dto => dto.MemberId, "Member", "MemberId"),
                new EntityField<FeeChargeResponse>(dto => dto.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<FeeChargeResponse>(dto => dto.AmountPaid, "Paid", "AmountPaid", typeof(decimal)),
                new EntityField<FeeChargeResponse>(dto => dto.Outstanding, "Outstanding", "Outstanding", typeof(decimal)),
                new EntityField<FeeChargeResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<FeeChargeResponse>(dto => dto.DueDate, "Due Date", "DueDate", typeof(DateTimeOffset)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchFeeChargesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchFeeChargesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<FeeChargeResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateFeeChargeAsync("1", viewModel.Adapt<CreateFeeChargeCommand>()).ConfigureAwait(false);
            },
            entityName: "Fee Charge",
            entityNamePlural: "Fee Charges",
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
        await DialogService.ShowAsync<FeeChargesHelpDialog>("Fee Charges Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View charge details in a dialog.
    /// </summary>
    private async Task ViewChargeDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(FeeChargeDetailsDialog.ChargeId), id }
        };

        await DialogService.ShowAsync<FeeChargeDetailsDialog>("Charge Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Waive a fee charge.
    /// </summary>
    private async Task WaiveFee(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(FeeChargeWaiveDialog.ChargeId), id }
        };

        var dialog = await DialogService.ShowAsync<FeeChargeWaiveDialog>("Waive Fee", parameters, new DialogOptions
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
