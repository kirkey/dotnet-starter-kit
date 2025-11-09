namespace FSH.Starter.Blazor.Client.Pages.Accounting.ArAccounts;

/// <summary>
/// AR Accounts page for managing accounts receivable subsidiary ledger.
/// </summary>
public partial class ArAccounts
{
    /// <summary>
    /// The entity table context for managing AR accounts.
    /// </summary>
    protected EntityServerTableContext<ArAccountResponse, DefaultIdType, ArAccountViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component.
    /// </summary>
    private EntityTable<ArAccountResponse, DefaultIdType, ArAccountViewModel> _table = null!;
    
    // Search filters
    private string? SearchAccountNumber { get; set; }
    private DefaultIdType? SearchCustomerId { get; set; }
    private bool SearchActiveOnly { get; set; } = true;

    /// <summary>
    /// Initializes the component and sets up the entity table context.
    /// </summary>
    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<ArAccountResponse, DefaultIdType, ArAccountViewModel>(
            entityName: "AR Account",
            entityNamePlural: "AR Accounts",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<ArAccountResponse>(ar => ar.AccountNumber, "Account #", "AccountNumber"),
                new EntityField<ArAccountResponse>(ar => ar.AccountName, "Account Name", "AccountName"),
                new EntityField<ArAccountResponse>(ar => ar.CustomerId, "Customer", "CustomerId"),
                new EntityField<ArAccountResponse>(ar => ar.Balance, "Balance", "Balance", typeof(decimal)),
                new EntityField<ArAccountResponse>(ar => ar.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: ar => ar.Id,
            searchFunc: async filter =>
            {
                // TODO: Implement when API endpoint is available
                await Task.CompletedTask;
                return new PaginationResponse<ArAccountResponse>
                {
                    Items = new List<ArAccountResponse>(),
                    TotalCount = 0
                };
            },
            createFunc: async viewModel =>
            {
                // TODO: Implement when API endpoint is available
                await Task.CompletedTask;
                Snackbar.Add($"AR Account {viewModel.AccountNumber} created successfully", Severity.Success);
            },
            updateFunc: null, // AR accounts are updated through specific workflows
            deleteFunc: null, // AR accounts should not be deleted
            hasExtraActionsFunc: () => true);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Opens the details dialog to view AR account details.
    /// </summary>
    private async Task OnViewDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ArAccountDetailsDialog.ArAccountId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        await DialogService.ShowAsync<ArAccountDetailsDialog>(
            "AR Account Details",
            parameters,
            options);
    }

    /// <summary>
    /// Opens the dialog to update AR balance and aging.
    /// </summary>
    private async Task OnUpdateBalance(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ArAccountUpdateBalanceDialog.ArAccountId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<ArAccountUpdateBalanceDialog>(
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
    /// Opens the dialog to record a collection (payment received).
    /// </summary>
    private async Task OnRecordCollection(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ArAccountRecordCollectionDialog.ArAccountId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<ArAccountRecordCollectionDialog>(
            "Record Collection",
            parameters,
            options);

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Opens the dialog to record a bad debt write-off.
    /// </summary>
    private async Task OnRecordWriteOff(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ArAccountRecordWriteOffDialog.ArAccountId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<ArAccountRecordWriteOffDialog>(
            "Record Write-Off",
            parameters,
            options);

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Opens the dialog to update allowance for doubtful accounts.
    /// </summary>
    private async Task OnUpdateAllowance(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ArAccountUpdateAllowanceDialog.ArAccountId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<ArAccountUpdateAllowanceDialog>(
            "Update Allowance",
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
            { nameof(ArAccountReconcileDialog.ArAccountId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<ArAccountReconcileDialog>(
            "Reconcile Account",
            parameters,
            options);

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }
}

