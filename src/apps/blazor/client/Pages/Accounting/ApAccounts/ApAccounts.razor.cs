namespace FSH.Starter.Blazor.Client.Pages.Accounting.ApAccounts;

/// <summary>
/// AP Accounts page for managing accounts payable subsidiary ledger.
/// </summary>
public partial class ApAccounts
{
    /// <summary>
    /// The entity table context for managing AP accounts.
    /// </summary>
    protected EntityServerTableContext<ApAccountResponse, DefaultIdType, ApAccountViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component.
    /// </summary>
    private EntityTable<ApAccountResponse, DefaultIdType, ApAccountViewModel> _table = null!;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

    // Search filters
    private string? SearchAccountNumber { get; set; }
    private DefaultIdType? SearchVendorId { get; set; }
    private bool SearchActiveOnly { get; set; } = true;

    /// <summary>
    /// Gets the status color based on account status.
    /// </summary>
    private static Color GetStatusColor(bool isActive) => isActive ? Color.Success : Color.Default;

    /// <summary>
    /// Initializes the component and sets up the entity table context.
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

        Context = new EntityServerTableContext<ApAccountResponse, DefaultIdType, ApAccountViewModel>(
            entityName: "AP Account",
            entityNamePlural: "AP Accounts",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<ApAccountResponse>(ap => ap.AccountNumber, "Account Number", "AccountNumber"),
                new EntityField<ApAccountResponse>(ap => ap.AccountName, "Account Name", "AccountName"),
                new EntityField<ApAccountResponse>(ap => ap.AccountType, "Type", "AccountType"),
                new EntityField<ApAccountResponse>(ap => ap.CurrentBalance, "Balance", "CurrentBalance", Type: typeof(decimal)),
                new EntityField<ApAccountResponse>(ap => ap.IsActive, "Status", "IsActive"),
            ],
            enableAdvancedSearch: true,
            idFunc: ap => ap.Id,
            searchFunc: async filter =>
            {
                // TODO: Implement when API endpoint is available
                await Task.CompletedTask;
                return new PaginationResponse<ApAccountResponse>
                {
                    Items = new List<ApAccountResponse>(),
                    TotalCount = 0
                };
            },
            createFunc: async viewModel =>
            {
                var command = new AccountsPayableAccountCreateCommand
                {
                    AccountNumber = viewModel.AccountNumber,
                    AccountName = viewModel.AccountName,
                    GeneralLedgerAccountId = viewModel.GeneralLedgerAccountId,
                    PeriodId = viewModel.PeriodId,
                    Description = viewModel.Description,
                    Notes = viewModel.Notes
                };

                await Client.ApAccountCreateEndpointAsync("1", command);
                Snackbar.Add($"AP Account {viewModel.AccountNumber} created successfully", Severity.Success);
            },
            updateFunc: null, // Update not yet implemented
            deleteFunc: null); // Delete not yet implemented

        base.OnInitialized();
    }

    /// <summary>
    /// Opens the details dialog to view AP account details.
    /// </summary>
    private async Task OnViewDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApAccountDetailsDialog.ApAccountId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        await DialogService.ShowAsync<ApAccountDetailsDialog>(
            "AP Account Details",
            parameters,
            options);
    }

    /// <summary>
    /// Opens the dialog to update AP balance and aging.
    /// </summary>
    private async Task OnUpdateBalance(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApAccountUpdateBalanceDialog.ApAccountId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<ApAccountUpdateBalanceDialog>(
            "Update Balance",
            parameters,
            options);

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Opens the dialog to record a payment made.
    /// </summary>
    private async Task OnRecordPayment(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApAccountRecordPaymentDialog.ApAccountId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<ApAccountRecordPaymentDialog>(
            "Record Payment",
            parameters,
            options);

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Opens the dialog to record discount lost.
    /// </summary>
    private async Task OnRecordDiscountLost(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApAccountRecordDiscountLostDialog.ApAccountId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<ApAccountRecordDiscountLostDialog>(
            "Record Discount Lost",
            parameters,
            options);

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Opens the dialog to reconcile with subsidiary ledger.
    /// </summary>
    private async Task OnReconcile(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApAccountReconcileDialog.ApAccountId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<ApAccountReconcileDialog>(
            "Reconcile Account",
            parameters,
            options);

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowApAccountsHelp()
    {
        await DialogService.ShowAsync<ApAccountsHelpDialog>("Accounts Payable Help", new DialogParameters(), _helpDialogOptions);
    }
}
