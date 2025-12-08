namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Loans;

/// <summary>
/// Loans page logic. Provides CRUD and search over Loan entities using the generated API client.
/// Manages loan applications, approvals, disbursements, and repayments.
/// </summary>
public partial class Loans
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<LoanResponse, DefaultIdType, LoanViewModel> Context { get; set; } = null!;

    private EntityTable<LoanResponse, DefaultIdType, LoanViewModel> _table = null!;

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
    private bool _canReject;
    private bool _canDisburse;
    private bool _canRecordRepayment;

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

    private string? _searchMemberName;
    private string? SearchMemberName
    {
        get => _searchMemberName;
        set
        {
            _searchMemberName = value;
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

        Context = new EntityServerTableContext<LoanResponse, DefaultIdType, LoanViewModel>(
            fields:
            [
                new EntityField<LoanResponse>(dto => dto.LoanNumber, "Loan #", "LoanNumber"),
                new EntityField<LoanResponse>(dto => dto.MemberName, "Member", "MemberName"),
                new EntityField<LoanResponse>(dto => dto.LoanProductName, "Product", "LoanProductName"),
                new EntityField<LoanResponse>(dto => dto.PrincipalAmount, "Principal", "PrincipalAmount", typeof(decimal)),
                new EntityField<LoanResponse>(dto => dto.InterestRate, "Rate %", "InterestRate", typeof(decimal)),
                new EntityField<LoanResponse>(dto => dto.TermMonths, "Term", "TermMonths", typeof(int)),
                new EntityField<LoanResponse>(dto => dto.OutstandingPrincipal, "Outstanding", "OutstandingPrincipal", typeof(decimal)),
                new EntityField<LoanResponse>(dto => dto.ApplicationDate, "Applied", "ApplicationDate", typeof(DateOnly)),
                new EntityField<LoanResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoansCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLoansAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateLoanAsync("1", viewModel.Adapt<CreateLoanCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateLoanAsync("1", id, viewModel.Adapt<UpdateLoanCommand>()).ConfigureAwait(false);
            },
            deleteFunc: null, // Loans should not be deleted
            entityName: "Loan",
            entityNamePlural: "Loans",
            entityResource: FshResources.Loans,
            hasExtraActionsFunc: () => true); // Always show menu for View Details/Schedule

        // Check permissions for extra actions
        var state = await AuthState;
        _canApprove = await AuthService.HasPermissionAsync(state.User, FshActions.Approve, FshResources.Loans);
        _canReject = await AuthService.HasPermissionAsync(state.User, FshActions.Reject, FshResources.Loans);
        _canDisburse = await AuthService.HasPermissionAsync(state.User, FshActions.Disburse, FshResources.Loans);
        _canRecordRepayment = await AuthService.HasPermissionAsync(state.User, FshActions.Create, FshResources.LoanRepayments);
    }

    private Color GetStatusColor(string status) => status switch
    {
        "Pending" => Color.Warning,
        "Approved" => Color.Info,
        "Disbursed" => Color.Primary,
        "Active" => Color.Success,
        "Completed" => Color.Default,
        "Defaulted" => Color.Error,
        "Rejected" => Color.Error,
        _ => Color.Default
    };

    /// <summary>
    /// Show loans help dialog.
    /// </summary>
    private async Task ShowLoansHelp()
    {
        await DialogService.ShowAsync<LoansHelpDialog>("Loans Management Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
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
            "Are you sure you want to approve this loan application?",
            yesText: "Approve",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ApproveLoanAsync("1", id, new ApproveLoanCommand()),
                Snackbar,
                successMessage: "Loan approved successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Reject a pending loan.
    /// </summary>
    private async Task RejectLoan(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Reject Loan",
            "Are you sure you want to reject this loan application?",
            yesText: "Reject",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.RejectLoanAsync("1", id, new RejectLoanCommand { Reason = "Rejected by officer" }),
                Snackbar,
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
            "Are you sure you want to disburse this loan? This will release funds to the member.",
            yesText: "Disburse",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.DisburseLoanAsync("1", id, new DisburseLoanCommand()),
                Snackbar,
                successMessage: "Loan disbursed successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Record a repayment for an active loan.
    /// </summary>
    private async Task RecordRepayment(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanRepaymentDialog.LoanId), id }
        };

        var dialog = await DialogService.ShowAsync<LoanRepaymentDialog>("Record Repayment", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        });

        var result = await dialog.Result;
        if (result?.Canceled == false)
        {
            await _table.ReloadDataAsync();
        }
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
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View loan repayment schedule.
    /// </summary>
    private async Task ViewSchedule(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanScheduleDialog.LoanId), id }
        };

        await DialogService.ShowAsync<LoanScheduleDialog>("Repayment Schedule", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
