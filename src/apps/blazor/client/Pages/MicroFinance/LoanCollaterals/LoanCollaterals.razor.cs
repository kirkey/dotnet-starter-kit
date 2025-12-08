namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanCollaterals;

/// <summary>
/// LoanCollaterals page logic. Provides CRUD and search over LoanCollateral entities using the generated API client.
/// Manages collateral assets pledged against loans through the verification and pledge workflow.
/// </summary>
public partial class LoanCollaterals
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<LoanCollateralResponse, DefaultIdType, LoanCollateralViewModel> Context { get; set; } = null!;

    private EntityTable<LoanCollateralResponse, DefaultIdType, LoanCollateralViewModel> _table = null!;

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

    // Date picker backing field
    private DateTime? _valuationDate = DateTime.Today;

    // Advanced search filters
    private string? _searchCollateralType;
    private string? SearchCollateralType
    {
        get => _searchCollateralType;
        set
        {
            _searchCollateralType = value;
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

    /// <summary>
    /// Initializes the table context with loan collateral-specific configuration.
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

        Context = new EntityServerTableContext<LoanCollateralResponse, DefaultIdType, LoanCollateralViewModel>(
            fields:
            [
                new EntityField<LoanCollateralResponse>(dto => dto.CollateralType, "Type", "CollateralType"),
                new EntityField<LoanCollateralResponse>(dto => dto.Description, "Description", "Description"),
                new EntityField<LoanCollateralResponse>(dto => dto.EstimatedValue, "Estimated Value", "EstimatedValue", typeof(decimal)),
                new EntityField<LoanCollateralResponse>(dto => dto.ForcedSaleValue, "Forced Sale", "ForcedSaleValue", typeof(decimal?)),
                new EntityField<LoanCollateralResponse>(dto => dto.ValuationDate, "Valuation Date", "ValuationDate", typeof(DateTimeOffset)),
                new EntityField<LoanCollateralResponse>(dto => dto.Location, "Location", "Location"),
                new EntityField<LoanCollateralResponse>(dto => dto.DocumentReference, "Document Ref", "DocumentReference"),
                new EntityField<LoanCollateralResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoanCollateralsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLoanCollateralsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanCollateralResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                // Set valuation date from date picker
                if (_valuationDate.HasValue)
                {
                    viewModel.ValuationDate = new DateTimeOffset(_valuationDate.Value);
                }
                
                await Client.CreateLoanCollateralAsync("1", viewModel.Adapt<CreateLoanCollateralCommand>()).ConfigureAwait(false);
            },
            // No update or delete for collaterals - they follow a workflow
            entityName: "Loan Collateral",
            entityNamePlural: "Loan Collaterals",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canManage);

        // Check permissions for extra actions
        var state = await AuthState;
        _canManage = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show loan collaterals help dialog.
    /// </summary>
    private async Task ShowLoanCollateralsHelp()
    {
        await DialogService.ShowAsync<LoanCollateralsHelpDialog>("Loan Collaterals Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View collateral details in a dialog.
    /// </summary>
    private async Task ViewCollateralDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanCollateralDetailsDialog.CollateralId), id }
        };

        await DialogService.ShowAsync<LoanCollateralDetailsDialog>("Collateral Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Verify a pending collateral.
    /// </summary>
    private async Task VerifyCollateral(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Verify Collateral",
            "Are you sure you want to verify this collateral? This confirms the valuation is accurate.",
            yesText: "Verify",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.VerifyLoanCollateralAsync("1", id),
                successMessage: "Collateral verified successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Pledge a verified collateral.
    /// </summary>
    private async Task PledgeCollateral(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Pledge Collateral",
            "Are you sure you want to pledge this collateral? This locks the asset as security for the loan.",
            yesText: "Pledge",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.PledgeLoanCollateralAsync("1", id),
                successMessage: "Collateral pledged successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Release a pledged collateral.
    /// </summary>
    private async Task ReleaseCollateral(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Release Collateral",
            "Are you sure you want to release this collateral? This removes the lien on the asset.",
            yesText: "Release",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ReleaseLoanCollateralAsync("1", id),
                successMessage: "Collateral released successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Seize a pledged collateral (for defaulted loans).
    /// </summary>
    private async Task SeizeCollateral(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Seize Collateral",
            "Are you sure you want to seize this collateral? This is for defaulted loans only.",
            yesText: "Seize",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.SeizeLoanCollateralAsync("1", id),
                successMessage: "Collateral seized.");
            await _table.ReloadDataAsync();
        }
    }
}
