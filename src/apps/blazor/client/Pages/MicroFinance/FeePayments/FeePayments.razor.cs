namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FeePayments;

/// <summary>
/// FeePayments page logic. Provides CRUD and search operations for FeePayment entities.
/// Manages payments made against fee charges with reversal capability.
/// </summary>
public partial class FeePayments
{
    static FeePayments()
    {
        // Configure Mapster to convert DateTimeOffset? to DateTime? for FeePaymentSummaryResponse -> FeePaymentViewModel mapping
        TypeAdapterConfig<FeePaymentSummaryResponse, FeePaymentViewModel>.NewConfig()
            .Map(dest => dest.PaymentDate, src => src.PaymentDate.HasValue ? src.PaymentDate.Value.DateTime : (DateTime?)null);
    }

    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<FeePaymentSummaryResponse, DefaultIdType, FeePaymentViewModel> Context { get; set; } = null!;

    private EntityTable<FeePaymentSummaryResponse, DefaultIdType, FeePaymentViewModel> _table = null!;

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
    private bool _canReverse;

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

    private string? _searchPaymentMethod;
    private string? SearchPaymentMethod
    {
        get => _searchPaymentMethod;
        set
        {
            _searchPaymentMethod = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with fee payment-specific configuration.
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

        Context = new EntityServerTableContext<FeePaymentSummaryResponse, DefaultIdType, FeePaymentViewModel>(
            fields:
            [
                new EntityField<FeePaymentSummaryResponse>(dto => dto.Reference, "Reference", "Reference"),
                new EntityField<FeePaymentSummaryResponse>(dto => dto.FeeChargeId, "Fee Charge", "FeeChargeId"),
                new EntityField<FeePaymentSummaryResponse>(dto => dto.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<FeePaymentSummaryResponse>(dto => dto.PaymentMethod, "Method", "PaymentMethod"),
                new EntityField<FeePaymentSummaryResponse>(dto => dto.PaymentSource, "Source", "PaymentSource"),
                new EntityField<FeePaymentSummaryResponse>(dto => dto.PaymentDate, "Payment Date", "PaymentDate", typeof(DateTimeOffset)),
                new EntityField<FeePaymentSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchFeePaymentsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchFeePaymentsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<FeePaymentSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateFeePaymentAsync("1", viewModel.Adapt<CreateFeePaymentCommand>()).ConfigureAwait(false);
            },
            entityName: "Fee Payment",
            entityNamePlural: "Fee Payments",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canReverse);

        // Check permissions
        var state = await AuthState;
        _canReverse = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show help dialog.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<FeePaymentsHelpDialog>("Fee Payments Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View payment details in a dialog.
    /// </summary>
    private async Task ViewDetails(DefaultIdType id)
    {
        var payment = await Client.GetFeePaymentAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters
        {
            { nameof(FeePaymentDetailsDialog.Payment), payment }
        };

        await DialogService.ShowAsync<FeePaymentDetailsDialog>("Payment Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Reverse a fee payment.
    /// </summary>
    private async Task ReversePayment(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(FeePaymentReverseDialog.PaymentId), id }
        };

        var dialog = await DialogService.ShowAsync<FeePaymentReverseDialog>("Reverse Payment", parameters, new DialogOptions
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
