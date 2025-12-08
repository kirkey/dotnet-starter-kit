namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Loans;

/// <summary>
/// Loans page logic. Provides CRUD and search over Loan entities using the generated API client.
/// Manages member loans including applications, approvals, disbursements, and status tracking.
/// </summary>
public partial class Loans
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<LoanSummaryResponse, DefaultIdType, LoanViewModel> Context { get; set; } = null!;

    private EntityTable<LoanSummaryResponse, DefaultIdType, LoanViewModel> _table = null!;

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
    private bool _canDisburse;
    private bool _canClose;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchLoanNumber;
    private string? SearchLoanNumber
    {
        get => _searchLoanNumber;
        set
        {
            _searchLoanNumber = value;
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
    /// Initializes the table context with loan-specific configuration.
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

        Context = new EntityServerTableContext<LoanSummaryResponse, DefaultIdType, LoanViewModel>(
            fields:
            [
                new EntityField<LoanSummaryResponse>(dto => dto.LoanNumber, "Loan #", "LoanNumber"),
                new EntityField<LoanSummaryResponse>(dto => dto.MemberName, "Member", "MemberName"),
                new EntityField<LoanSummaryResponse>(dto => dto.LoanProductName, "Product", "LoanProductName"),
                new EntityField<LoanSummaryResponse>(dto => dto.PrincipalAmount, "Principal", "PrincipalAmount", typeof(decimal)),
                new EntityField<LoanSummaryResponse>(dto => dto.InterestRate, "Rate %", "InterestRate", typeof(decimal)),
                new EntityField<LoanSummaryResponse>(dto => dto.TermMonths, "Term", "TermMonths", typeof(int)),
                new EntityField<LoanSummaryResponse>(dto => dto.OutstandingPrincipal, "Outstanding", "OutstandingPrincipal", typeof(decimal)),
                new EntityField<LoanSummaryResponse>(dto => dto.ApplicationDate, "Applied", "ApplicationDate", typeof(DateTimeOffset)),
                new EntityField<LoanSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoansCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    LoanNumber = _searchLoanNumber,
                    Status = _searchStatus
                };
                var result = await Client.SearchLoansAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                // Sync IDs from autocomplete selections
                viewModel.SyncIdsFromSelections();
                await Client.CreateLoanAsync("1", viewModel.Adapt<CreateLoanCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                viewModel.SyncIdsFromSelections();
                await Client.UpdateLoanAsync("1", id, viewModel.Adapt<UpdateLoanCommand>()).ConfigureAwait(false);
            },
            // No delete for loans - they are closed or written off
            entityName: "Loan",
            entityNamePlural: "Loans",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canApprove || _canDisburse || _canClose);

        // Check permissions for extra actions
        var state = await AuthState;
        _canApprove = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
        _canDisburse = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
        _canClose = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show loans help dialog.
    /// </summary>
    private async Task ShowLoansHelp()
    {
        await DialogService.ShowAsync<LoansHelpDialog>("Loans Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View loan details in a dialog.
    /// </summary>
    private async Task ViewLoanDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanDetailsDialog.LoanId), id }
        };

        await DialogService.ShowAsync<LoanDetailsDialog>("Loan Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Approve a pending loan.
    /// </summary>
    private async Task ApproveLoan(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Approve Loan",
            "Are you sure you want to approve this loan?",
            yesText: "Approve",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ApproveLoanAsync("1", id, new ApproveLoanCommand()),
                successMessage: "Loan approved successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Reject a pending loan.
    /// </summary>
    private async Task RejectLoan(DefaultIdType id)
    {
        var reason = await DialogService.ShowMessageBox(
            "Reject Loan",
            "Are you sure you want to reject this loan?",
            yesText: "Reject",
            cancelText: "Cancel");

        if (reason == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.RejectLoanAsync("1", id, new RejectLoanCommand { RejectionReason = "Rejected by administrator" }),
                successMessage: "Loan rejected.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Disburse an approved loan.
    /// </summary>
    private async Task DisburseLoan(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Disburse Loan",
            "Are you sure you want to disburse this loan? Funds will be released to the member.",
            yesText: "Disburse",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.DisburseLoanAsync("1", id, new DisburseLoanCommand { DisbursementMethod = "BANK_TRANSFER", Notes = "Disbursed by administrator" }),
                successMessage: "Loan disbursed successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Close a fully paid loan.
    /// </summary>
    private async Task CloseLoan(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Close Loan",
            "Are you sure you want to close this loan? This action cannot be undone.",
            yesText: "Close",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.CloseLoanAsync("1", id),
                successMessage: "Loan closed successfully.");
            await _table.ReloadDataAsync();
        }
    }
}
