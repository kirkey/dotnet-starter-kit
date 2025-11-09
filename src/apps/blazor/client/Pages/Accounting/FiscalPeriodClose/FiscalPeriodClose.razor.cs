namespace FSH.Starter.Blazor.Client.Pages.Accounting.FiscalPeriodClose;

/// <summary>
/// Fiscal Period Close page for managing month-end, quarter-end, and year-end closing processes.
/// </summary>
public partial class FiscalPeriodClose
{
    /// <summary>
    /// The entity table context for managing fiscal period closes.
    /// </summary>
    protected EntityServerTableContext<FiscalPeriodCloseResponse, DefaultIdType, FiscalPeriodCloseViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component.
    /// </summary>
    private EntityTable<FiscalPeriodCloseResponse, DefaultIdType, FiscalPeriodCloseViewModel> _table = null!;

    /// <summary>
    /// Reference to the checklist dialog.
    /// </summary>
    private FiscalPeriodCloseChecklistDialog _checklistDialog = null!;

    /// <summary>
    /// Reference to the reopen dialog.
    /// </summary>
    private FiscalPeriodCloseReopenDialog _reopenDialog = null!;

    /// <summary>
    /// Search filter for close number.
    /// </summary>
    private string? SearchCloseNumber { get; set; }

    /// <summary>
    /// Search filter for close type (MonthEnd, QuarterEnd, YearEnd).
    /// </summary>
    private string? SearchCloseType { get; set; }

    /// <summary>
    /// Search filter for status (InProgress, Completed, Reopened).
    /// </summary>
    private string? SearchStatus { get; set; }

    /// <summary>
    /// Gets the status color based on period close status.
    /// </summary>
    private static Color GetStatusColor(string? status) => status switch
    {
        "Completed" => Color.Success,
        "InProgress" => Color.Info,
        "Reopened" => Color.Warning,
        _ => Color.Default
    };

    /// <summary>
    /// Initializes the component and sets up the entity table context.
    /// </summary>
    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<FiscalPeriodCloseResponse, DefaultIdType, FiscalPeriodCloseViewModel>(
            entityName: "Fiscal Period Close",
            entityNamePlural: "Fiscal Period Closes",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<FiscalPeriodCloseResponse>(pc => pc.CloseNumber, "Close Number", "CloseNumber"),
                new EntityField<FiscalPeriodCloseResponse>(pc => pc.CloseType, "Close Type", "CloseType"),
                new EntityField<FiscalPeriodCloseResponse>(pc => pc.PeriodStartDate, "Start Date", "PeriodStartDate", typeof(DateOnly)),
                new EntityField<FiscalPeriodCloseResponse>(pc => pc.PeriodEndDate, "End Date", "PeriodEndDate", typeof(DateOnly)),
                new EntityField<FiscalPeriodCloseResponse>(pc => pc.Status, "Status", "Status"),
                new EntityField<FiscalPeriodCloseResponse>(pc => pc.CloseDate, "Close Date", "CloseDate", typeof(DateTime?)),
            ],
            enableAdvancedSearch: true,
            idFunc: pc => pc.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchFiscalPeriodClosesRequest>();
                var result = await Client.FiscalPeriodCloseSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<FiscalPeriodCloseResponse>>();
            },
            createFunc: async viewModel =>
            {
                var command = new FiscalPeriodCloseCreateCommand
                {
                    CloseNumber = viewModel.CloseNumber,
                    PeriodId = viewModel.PeriodId!.Value,
                    CloseType = viewModel.CloseType,
                    PeriodStartDate = viewModel.PeriodStartDate!.Value,
                    PeriodEndDate = viewModel.PeriodEndDate!.Value,
                    InitiatedBy = "Current User", // TODO: Get from auth context
                    Description = viewModel.Description,
                    Notes = viewModel.Notes
                };

                await Client.FiscalPeriodCloseCreateEndpointAsync("1", command);
                Snackbar.Add($"Fiscal Period Close {viewModel.CloseNumber} initiated successfully", Severity.Success);
            },
            updateFunc: null, // Period closes don't support direct updates
            deleteFunc: null); // Period closes don't support deletion

        base.OnInitialized();
    }

    /// <summary>
    /// Opens the checklist dialog to view and manage close tasks.
    /// </summary>
    private async Task OnViewChecklist(DefaultIdType id)
    {
        await _checklistDialog.ShowAsync(id);
    }

    /// <summary>
    /// Completes the fiscal period close process.
    /// </summary>
    private async Task OnComplete(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox(
            "Complete Period Close",
            "Are you sure you want to complete this period close? All required tasks must be completed. This action will lock the period.",
            yesText: "Complete",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                var command = new CompleteFiscalPeriodCloseCommand
                {
                    FiscalPeriodCloseId = id,
                    CompletedBy = "Current User" // TODO: Get from auth context
                };

                await Client.CompleteFiscalPeriodCloseEndpointAsync("1", id, command);
                Snackbar.Add("Fiscal period close completed successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error completing period close: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Opens the reopen dialog to reopen a completed period close.
    /// </summary>
    private async Task OnReopen(DefaultIdType id)
    {
        await _reopenDialog.ShowAsync(id);
    }

    /// <summary>
    /// Views the period close report.
    /// </summary>
    private async Task OnViewReport(DefaultIdType id)
    {
        try
        {
            Snackbar.Add("Generating period close report...", Severity.Info);
            // TODO: Implement report generation when API endpoint is available
            await Task.Delay(500); // Placeholder
            Snackbar.Add("Report feature coming soon", Severity.Warning);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error generating report: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Callback when a task is completed.
    /// </summary>
    private async Task OnTaskCompletedCallback()
    {
        await _table.ReloadDataAsync();
    }

    /// <summary>
    /// Callback when a period is reopened.
    /// </summary>
    private async Task OnReopenedCallback()
    {
        await _table.ReloadDataAsync();
    }
}

