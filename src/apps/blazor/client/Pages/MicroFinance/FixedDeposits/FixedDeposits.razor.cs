namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FixedDeposits;

/// <summary>
/// FixedDeposits page logic. Provides CRUD and search over FixedDeposit entities using the generated API client.
/// Manages fixed deposit accounts including interest posting, maturity processing, and renewals.
/// </summary>
public partial class FixedDeposits
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<FixedDepositResponse, DefaultIdType, FixedDepositViewModel> Context { get; set; } = null!;

    private EntityTable<FixedDepositResponse, DefaultIdType, FixedDepositViewModel> _table = null!;

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
    private DateTime? _depositDate = DateTime.Today;

    // Advanced search filters
    private string? _searchCertificateNumber;
    private string? SearchCertificateNumber
    {
        get => _searchCertificateNumber;
        set
        {
            _searchCertificateNumber = value;
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
    /// Initializes the table context with fixed deposit-specific configuration.
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

        Context = new EntityServerTableContext<FixedDepositResponse, DefaultIdType, FixedDepositViewModel>(
            fields:
            [
                new EntityField<FixedDepositResponse>(dto => dto.CertificateNumber, "Certificate #", "CertificateNumber"),
                new EntityField<FixedDepositResponse>(dto => dto.PrincipalAmount, "Principal", "PrincipalAmount", typeof(decimal)),
                new EntityField<FixedDepositResponse>(dto => dto.InterestRate, "Rate %", "InterestRate", typeof(decimal)),
                new EntityField<FixedDepositResponse>(dto => dto.TermMonths, "Term", "TermMonths", typeof(int)),
                new EntityField<FixedDepositResponse>(dto => dto.DepositDate, "Deposit Date", "DepositDate", typeof(DateTimeOffset)),
                new EntityField<FixedDepositResponse>(dto => dto.MaturityDate, "Maturity Date", "MaturityDate", typeof(DateTimeOffset)),
                new EntityField<FixedDepositResponse>(dto => dto.InterestEarned, "Interest Earned", "InterestEarned", typeof(decimal)),
                new EntityField<FixedDepositResponse>(dto => dto.InterestPaid, "Interest Paid", "InterestPaid", typeof(decimal)),
                new EntityField<FixedDepositResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchFixedDepositsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchFixedDepositsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<FixedDepositResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                // Sync IDs from autocomplete selections
                viewModel.SyncIdsFromSelections();
                
                // Set deposit date from date picker
                if (_depositDate.HasValue)
                {
                    viewModel.DepositDate = new DateTimeOffset(_depositDate.Value);
                }
                
                await Client.CreateFixedDepositAsync("1", viewModel.Adapt<CreateFixedDepositCommand>()).ConfigureAwait(false);
            },
            // No update or delete for fixed deposits - they mature, renew, or close
            entityName: "Fixed Deposit",
            entityNamePlural: "Fixed Deposits",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canManage);

        // Check permissions for extra actions
        var state = await AuthState;
        _canManage = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show fixed deposits help dialog.
    /// </summary>
    private async Task ShowFixedDepositsHelp()
    {
        await DialogService.ShowAsync<FixedDepositsHelpDialog>("Fixed Deposits Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View fixed deposit details in a dialog.
    /// </summary>
    private async Task ViewDepositDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(FixedDepositDetailsDialog.DepositId), id }
        };

        await DialogService.ShowAsync<FixedDepositDetailsDialog>("Fixed Deposit Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Post interest to a fixed deposit.
    /// </summary>
    private async Task PostInterest(DefaultIdType id)
    {
        // Get the deposit to calculate interest
        var deposit = await Client.GetFixedDepositAsync("1", id);
        var interestAmount = deposit.PrincipalAmount * (deposit.InterestRate / 100m) / 12m;

        var confirmed = await DialogService.ShowMessageBox(
            "Post Interest",
            $"Post monthly interest of {interestAmount:C2} to this fixed deposit?",
            yesText: "Post Interest",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.PostFixedDepositInterestAsync("1", id, new PostFixedDepositInterestCommand 
                { 
                    DepositId = id,
                    InterestAmount = interestAmount 
                }),
                successMessage: "Interest posted successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Pay interest to a member's account.
    /// </summary>
    private async Task PayInterest(DefaultIdType id)
    {
        var deposit = await Client.GetFixedDepositAsync("1", id);
        var unpaidInterest = deposit.InterestEarned - deposit.InterestPaid;

        if (unpaidInterest <= 0)
        {
            await DialogService.ShowMessageBox(
                "No Interest to Pay",
                "There is no unpaid interest for this deposit.",
                yesText: "OK");
            return;
        }

        var confirmed = await DialogService.ShowMessageBox(
            "Pay Interest",
            $"Pay interest of {unpaidInterest:C2} to the member?",
            yesText: "Pay Interest",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.PayFixedDepositInterestAsync("1", id, new PayFixedDepositInterestCommand 
                { 
                    DepositId = id,
                    Amount = unpaidInterest 
                }),
                successMessage: "Interest paid successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Process maturity of a fixed deposit.
    /// </summary>
    private async Task MatureDeposit(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Process Maturity",
            "Are you sure you want to process the maturity of this fixed deposit?",
            yesText: "Process Maturity",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.MatureFixedDepositAsync("1", id),
                successMessage: "Fixed deposit matured successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Renew a matured fixed deposit.
    /// </summary>
    private async Task RenewDeposit(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Renew Fixed Deposit",
            "Are you sure you want to renew this fixed deposit for another term?",
            yesText: "Renew",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.RenewFixedDepositAsync("1", id),
                successMessage: "Fixed deposit renewed successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Close a fixed deposit prematurely.
    /// </summary>
    private async Task ClosePremature(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Close Premature",
            "Are you sure you want to close this fixed deposit early? Penalties may apply.",
            yesText: "Close Early",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.CloseFixedDepositPrematureAsync("1", id, new ClosePrematureFixedDepositCommand 
                { 
                    DepositId = id,
                    Reason = "Closed prematurely by administrator" 
                }),
                successMessage: "Fixed deposit closed prematurely.");
            await _table.ReloadDataAsync();
        }
    }
}
