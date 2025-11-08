namespace FSH.Starter.Blazor.Client.Pages.Accounting.TrialBalance;

/// <summary>
/// Trial Balance page for generating and managing trial balance reports.
/// </summary>
public partial class TrialBalance
{
    /// <summary>
    /// The entity table context for managing trial balance reports.
    /// </summary>
    protected EntityServerTableContext<TrialBalanceSearchResponse, DefaultIdType, TrialBalanceViewModel> Context { get; set; } = default!;

    /// <summary>
    /// Reference to the EntityTable component.
    /// </summary>
    private EntityTable<TrialBalanceSearchResponse, DefaultIdType, TrialBalanceViewModel> _table = default!;

    /// <summary>
    /// Reference to the details dialog.
    /// </summary>
    private TrialBalanceDetailsDialog _detailsDialog = default!;

    // Search filters
    private string? TrialBalanceNumber { get; set; }
    private DefaultIdType? SearchPeriodId { get; set; }
    private string? SearchStatus { get; set; }
    private DateTime? SearchStartDate { get; set; }
    private DateTime? SearchEndDate { get; set; }
    private bool SearchBalancedOnly { get; set; }

    /// <summary>
    /// Initializes the component and sets up the entity table context.
    /// </summary>
    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<TrialBalanceSearchResponse, DefaultIdType, TrialBalanceViewModel>(
            entityName: "Trial Balance",
            entityNamePlural: "Trial Balance Reports",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<TrialBalanceSearchResponse>(tb => tb.TrialBalanceNumber, "Number", "TrialBalanceNumber"),
                new EntityField<TrialBalanceSearchResponse>(tb => tb.PeriodStartDate, "Start Date", "PeriodStartDate", typeof(DateOnly)),
                new EntityField<TrialBalanceSearchResponse>(tb => tb.PeriodEndDate, "End Date", "PeriodEndDate", typeof(DateOnly)),
                new EntityField<TrialBalanceSearchResponse>(tb => tb.TotalDebits, "Total Debits", "TotalDebits", typeof(decimal)),
                new EntityField<TrialBalanceSearchResponse>(tb => tb.TotalCredits, "Total Credits", "TotalCredits", typeof(decimal)),
                new EntityField<TrialBalanceSearchResponse>(tb => tb.IsBalanced, "Balanced", "IsBalanced", typeof(bool)),
                new EntityField<TrialBalanceSearchResponse>(tb => tb.Status, "Status", "Status"),
                new EntityField<TrialBalanceSearchResponse>(tb => tb.FinalizedDate, "Finalized", "FinalizedDate", typeof(DateOnly)),
            ],
            enableAdvancedSearch: true,
            idFunc: tb => tb.Id,
            searchFunc: async filter =>
            {
                var searchQuery = new TrialBalanceSearchQuery
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    TrialBalanceNumber = TrialBalanceNumber,
                    PeriodId = SearchPeriodId,
                    Status = SearchStatus,
                    StartDate = SearchStartDate,
                    EndDate = SearchEndDate,
                    IsBalanced = SearchBalancedOnly ? true : null
                };

                var result = await Client.TrialBalanceSearchEndpointAsync("1", searchQuery);
                return result.Adapt<PaginationResponse<TrialBalanceSearchResponse>>();
            },
            // getDetailsFunc: async id =>
            // {
            //     var trialBalance = await Client.TrialBalanceGetEndpointAsync("1", id);
            //     return new TrialBalanceViewModel
            //     {
            //         Id = trialBalance.Id,
            //         TrialBalanceNumber = trialBalance.TrialBalanceNumber,
            //         PeriodId = trialBalance.PeriodId,
            //         PeriodStartDate = trialBalance.PeriodStartDate,
            //         PeriodEndDate = trialBalance.PeriodEndDate,
            //         IncludeZeroBalances = trialBalance.IncludeZeroBalances,
            //         Description = trialBalance.Description,
            //         Notes = trialBalance.Notes
            //     };
            // },
            createFunc: async viewModel =>
            {
                var command = new TrialBalanceCreateCommand
                {
                    TrialBalanceNumber = viewModel.TrialBalanceNumber,
                    PeriodId = viewModel.PeriodId!.Value,
                    PeriodStartDate = viewModel.PeriodStartDate!.Value,
                    PeriodEndDate = viewModel.PeriodEndDate!.Value,
                    IncludeZeroBalances = viewModel.IncludeZeroBalances,
                    AutoGenerate = viewModel.AutoGenerate,
                    Description = viewModel.Description,
                    Notes = viewModel.Notes
                };

                await Client.TrialBalanceCreateEndpointAsync("1", command);
                Snackbar.Add($"Trial Balance {viewModel.TrialBalanceNumber} created successfully", Severity.Success);
            },
            updateFunc: null, // Trial balance doesn't support direct updates
            deleteFunc: null); // Trial balance doesn't support deletion

        base.OnInitialized();
    }

    /// <summary>
    /// Opens the details dialog to view trial balance report.
    /// </summary>
    private async Task OnViewDetails(DefaultIdType id)
    {
        await _detailsDialog.ShowAsync(id);
    }

    /// <summary>
    /// Finalizes a trial balance report.
    /// </summary>
    private async Task OnFinalize(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox(
            "Finalize Trial Balance",
            "Are you sure you want to finalize this trial balance? This action locks the report for the period.",
            yesText: "Finalize",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                var command = new TrialBalanceFinalizeCommand { Id = id };
                await Client.TrialBalanceFinalizeEndpointAsync("1", id, command);
                Snackbar.Add("Trial balance finalized successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error finalizing trial balance: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Reopens a finalized trial balance report.
    /// </summary>
    private async Task OnReopen(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox(
            "Reopen Trial Balance",
            "Are you sure you want to reopen this trial balance? This will allow modifications to the period.",
            yesText: "Reopen",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                var command = new TrialBalanceReopenCommand { Id = id };
                await Client.TrialBalanceReopenEndpointAsync("1", id, command);
                Snackbar.Add("Trial balance reopened successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error reopening trial balance: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Exports trial balance to Excel.
    /// </summary>
    private async Task OnExport(DefaultIdType id)
    {
        try
        {
            Snackbar.Add("Exporting trial balance to Excel...", Severity.Info);
            // TODO: Implement export functionality when API endpoint is available
            await Task.Delay(500); // Placeholder
            Snackbar.Add("Export feature coming soon", Severity.Warning);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error exporting trial balance: {ex.Message}", Severity.Error);
        }
    }
}

