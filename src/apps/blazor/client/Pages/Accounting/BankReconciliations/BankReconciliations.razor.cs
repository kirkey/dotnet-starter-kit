namespace FSH.Starter.Blazor.Client.Pages.Accounting.BankReconciliations;

/// <summary>
/// Bank Reconciliation page for managing bank statement reconciliations.
/// </summary>
public partial class BankReconciliations
{

    /// <summary>
    /// The entity table context for managing bank reconciliations with server-side operations.
    /// </summary>
    protected EntityServerTableContext<BankReconciliationResponse, DefaultIdType, BankReconciliationViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component for bank reconciliations.
    /// </summary>
    private EntityTable<BankReconciliationResponse, DefaultIdType, BankReconciliationViewModel> _table = null!;

    /// <summary>
    /// Search filter for bank account.
    /// </summary>
    private string? BankAccountFilter { get; set; }

    /// <summary>
    /// Search filter for reconciliation status.
    /// </summary>
    private string? Status { get; set; }

    /// <summary>
    /// Search filter for reconciliation date range start.
    /// </summary>
    private DateTime? ReconciliationDateFrom { get; set; }

    /// <summary>
    /// Search filter for reconciliation date range end.
    /// </summary>
    private DateTime? ReconciliationDateTo { get; set; }

    /// <summary>
    /// Search filter for reconciliation approved status.
    /// </summary>
    private bool IsReconciled { get; set; }


    /// <summary>
    /// ID of the reconciliation being edited.
    /// </summary>
    private DefaultIdType _selectedReconciliationId = DefaultIdType.Empty;

    /// <summary>
    /// Gets the status color based on reconciliation status.
    /// </summary>
    private static Color GetStatusColor(string? status) => status switch
    {
        "Pending" => Color.Default,
        "InProgress" => Color.Info,
        "Completed" => Color.Warning,
        "Approved" => Color.Success,
        _ => Color.Default
    };

    /// <summary>
    /// Gets the reconciliation badge color.
    /// </summary>
    private static Color GetReconciliationBadgeColor(bool isReconciled) =>
        isReconciled ? Color.Success : Color.Warning;

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<BankReconciliationResponse, DefaultIdType, BankReconciliationViewModel>(
            entityName: "Bank Reconciliation",
            entityNamePlural: "Bank Reconciliations",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<BankReconciliationResponse>(r => r.StatementNumber ?? "N/A", "Statement #", "StatementNumber"),
                new EntityField<BankReconciliationResponse>(r => r.ReconciliationDate, "Date", "ReconciliationDate", typeof(DateOnly)),
                new EntityField<BankReconciliationResponse>(r => r.StatementBalance, "Statement Balance", "StatementBalance", typeof(decimal)),
                new EntityField<BankReconciliationResponse>(r => r.BookBalance, "Book Balance", "BookBalance", typeof(decimal)),
                new EntityField<BankReconciliationResponse>(r => r.AdjustedBalance, "Adjusted Balance", "AdjustedBalance", typeof(decimal)),
                new EntityField<BankReconciliationResponse>(r => r.Status, "Status", "Status"),
                new EntityField<BankReconciliationResponse>(r => r.IsReconciled, "Approved", "IsReconciled", typeof(bool)),
                new EntityField<BankReconciliationResponse>(r => r.ReconciledDate, "Completed Date", "ReconciledDate", typeof(DateTime?)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchBankReconciliationsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchBankReconciliationsEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<BankReconciliationResponse>>();
            },
            createFunc: async reconciliation =>
            {
                await Client.CreateBankReconciliationEndpointAsync("1", reconciliation.Adapt<CreateBankReconciliationCommand>());
            },
            updateFunc: async (id, reconciliation) =>
            {
                var updateCommand = reconciliation.Adapt<UpdateBankReconciliationCommand>();
                updateCommand.Id = id;
                await Client.UpdateBankReconciliationEndpointAsync("1", id, updateCommand);
            },
            deleteFunc: async id => await Client.DeleteBankReconciliationEndpointAsync("1", id));

        return Task.CompletedTask;
    }


    /// <summary>
    /// View reconciliation details in a dialog.
    /// </summary>
    private async Task ViewReconciliationDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(BankReconciliationDetailsDialog.Id), id }
        };
        var result = await DialogService.ShowAsync<BankReconciliationDetailsDialog>("Reconciliation Details", parameters);
        await result.Result;
    }

    /// <summary>
    /// Start a bank reconciliation.
    /// </summary>
    private async Task OnStartReconciliation(DefaultIdType id)
    {
        try
        {
            await Client.StartBankReconciliationEndpointAsync("1", id);
            Snackbar.Add("Reconciliation started successfully.", Severity.Success);
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error starting reconciliation: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Edit reconciliation items in a dialog.
    /// </summary>
    private async Task OnEditReconciliation(DefaultIdType id)
    {
        _selectedReconciliationId = id;
        var parameters = new DialogParameters
        {
            { nameof(BankReconciliationEditDialog.Id), id }
        };
        var dialog = await DialogService.ShowAsync<BankReconciliationEditDialog>("Update Reconciliation Items", parameters);
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Complete a reconciliation.
    /// </summary>
    private async Task OnCompleteReconciliation(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(BankReconciliationCompleteDialog.ReconciliationId), id }
        };
        var dialog = await DialogService.ShowAsync<BankReconciliationCompleteDialog>("Complete Reconciliation", parameters);
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Approve a completed reconciliation.
    /// </summary>
    private async Task OnApproveReconciliation(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(BankReconciliationApproveDialog.ReconciliationId), id }
        };
        var dialog = await DialogService.ShowAsync<BankReconciliationApproveDialog>("Approve Reconciliation", parameters);
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Reject a completed reconciliation.
    /// </summary>
    private async Task OnRejectReconciliation(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(BankReconciliationRejectDialog.ReconciliationId), id }
        };
        var dialog = await DialogService.ShowAsync<BankReconciliationRejectDialog>("Reject Reconciliation", parameters);
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Delete a reconciliation.
    /// </summary>
    private async Task OnDeleteReconciliation(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), $"Are you sure you want to delete this bank reconciliation? This action cannot be undone." }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete Reconciliation", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            try
            {
                await Client.DeleteBankReconciliationEndpointAsync("1", id);
                Snackbar.Add("Reconciliation deleted successfully.", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error deleting reconciliation: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Print a reconciliation report.
    /// </summary>
    private async Task OnPrintReconciliation(DefaultIdType id)
    {
        Snackbar.Add("Print functionality coming soon.", Severity.Info);
    }

    /// <summary>
    /// Show reconciliation reports.
    /// </summary>
    private async Task ShowReconciliationReports()
    {
        var result = await DialogService.ShowAsync<BankReconciliationReportsDialog>("Reconciliation Reports", new DialogParameters());
        await result.Result;
    }

    /// <summary>
    /// Show reconciliation summary.
    /// </summary>
    private async Task ShowSummary()
    {
        var result = await DialogService.ShowAsync<BankReconciliationSummaryDialog>("Reconciliation Summary", new DialogParameters());
        await result.Result;
    }

    /// <summary>
    /// Show pending approvals.
    /// </summary>
    private async Task ShowPendingApprovals()
    {
        Status = "Completed";
        await _table.ReloadDataAsync();
    }

    /// <summary>
    /// Show in-progress reconciliations.
    /// </summary>
    private async Task ShowInProgressReconciliations()
    {
        Status = "InProgress";
        await _table.ReloadDataAsync();
    }

    /// <summary>
    /// Show completed reconciliations.
    /// </summary>
    private async Task ShowCompletedReconciliations()
    {
        Status = "Completed";
        await _table.ReloadDataAsync();
    }

    /// <summary>
    /// Export reconciliations.
    /// </summary>
    private async Task ExportReconciliations()
    {
        Snackbar.Add("Export functionality coming soon.", Severity.Info);
    }

    /// <summary>
    /// Show settings dialog.
    /// </summary>
    private async Task ShowSettings()
    {
        Snackbar.Add("Settings coming soon.", Severity.Info);
    }

    /// <summary>
    /// Show bank reconciliation help dialog.
    /// </summary>
    private async Task ShowBankReconciliationsHelp()
    {
        await DialogService.ShowAsync<BankReconciliationsHelpDialog>("Bank Reconciliation Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

