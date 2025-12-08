namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanRepayments;

/// <summary>
/// Loan Repayments page logic. Provides Create and Search operations over LoanRepayment entities.
/// Manages loan repayment transactions.
/// </summary>
public partial class LoanRepayments
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<LoanRepaymentResponse, DefaultIdType, LoanRepaymentViewModel> Context { get; set; } = null!;

    private EntityTable<LoanRepaymentResponse, DefaultIdType, LoanRepaymentViewModel> _table = null!;

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
    private string? _searchReceiptNumber;
    private string? SearchReceiptNumber
    {
        get => _searchReceiptNumber;
        set
        {
            _searchReceiptNumber = value;
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
    /// Initializes the table context with loan repayment-specific configuration.
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

        Context = new EntityServerTableContext<LoanRepaymentResponse, DefaultIdType, LoanRepaymentViewModel>(
            fields:
            [
                new EntityField<LoanRepaymentResponse>(dto => dto.ReceiptNumber, "Receipt #", "ReceiptNumber"),
                new EntityField<LoanRepaymentResponse>(dto => dto.LoanAccountNumber, "Loan Account", "LoanAccountNumber"),
                new EntityField<LoanRepaymentResponse>(dto => dto.MemberName, "Member", "MemberName"),
                new EntityField<LoanRepaymentResponse>(dto => dto.TotalAmount, "Total Amount", "TotalAmount", typeof(decimal)),
                new EntityField<LoanRepaymentResponse>(dto => dto.PrincipalAmount, "Principal", "PrincipalAmount", typeof(decimal)),
                new EntityField<LoanRepaymentResponse>(dto => dto.InterestAmount, "Interest", "InterestAmount", typeof(decimal)),
                new EntityField<LoanRepaymentResponse>(dto => dto.RepaymentDate, "Date", "RepaymentDate", typeof(DateTimeOffset)),
                new EntityField<LoanRepaymentResponse>(dto => dto.PaymentMethod, "Payment Method", "PaymentMethod"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoanRepaymentsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    PaymentMethod = _searchPaymentMethod
                };
                var result = await Client.SearchLoanRepaymentsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanRepaymentResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateLoanRepaymentAsync("1", viewModel.Adapt<CreateLoanRepaymentCommand>()).ConfigureAwait(false);
            },
            // No update or delete for loan repayments - they are financial transactions
            entityName: "Loan Repayment",
            entityNamePlural: "Loan Repayments",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => true);

        // Check permissions for extra actions
        var state = await AuthState;
        _canReverse = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show loan repayments help dialog.
    /// </summary>
    private async Task ShowLoanRepaymentsHelp()
    {
        await DialogService.ShowAsync<LoanRepaymentsHelpDialog>("Loan Repayments Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View repayment details in a dialog.
    /// </summary>
    private async Task ViewRepaymentDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanRepaymentDetailsDialog.RepaymentId), id }
        };

        await DialogService.ShowAsync<LoanRepaymentDetailsDialog>("Loan Repayment Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Print receipt for a repayment.
    /// </summary>
    private async Task PrintReceipt(DefaultIdType id)
    {
        // TODO: Implement receipt printing functionality
        await DialogService.ShowMessageBox(
            "Print Receipt",
            "Receipt printing functionality will be implemented in a future update.",
            yesText: "OK");
    }

    /// <summary>
    /// Reverse a loan repayment transaction.
    /// </summary>
    private async Task ReverseRepayment(DefaultIdType id)
    {
        var result = await DialogService.ShowMessageBox(
            "Reverse Repayment",
            "Are you sure you want to reverse this repayment? This will restore the loan balance and mark the repayment as reversed.",
            yesText: "Reverse",
            cancelText: "Cancel");

        if (result == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ReverseLoanRepaymentAsync("1", id, new ReverseLoanRepaymentCommand
                {
                    LoanRepaymentId = id,
                    Reason = "Reversed by administrator"
                }),
                successMessage: "Repayment reversed successfully.");
            await _table.ReloadDataAsync();
        }
    }
}
