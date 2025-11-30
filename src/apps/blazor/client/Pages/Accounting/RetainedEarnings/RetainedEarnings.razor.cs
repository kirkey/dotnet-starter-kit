namespace FSH.Starter.Blazor.Client.Pages.Accounting.RetainedEarnings;

/// <summary>
/// Retained Earnings page for managing annual equity records.
/// </summary>
public partial class RetainedEarnings
{
    [Inject] protected ICourier Courier { get; set; } = null!;
    
    private ClientPreference _preference = new();
    /// <summary>
    /// The entity table context for managing retained earnings.
    /// </summary>
    protected EntityServerTableContext<RetainedEarningsResponse, DefaultIdType, RetainedEarningsViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component.
    /// </summary>
    private EntityTable<RetainedEarningsResponse, DefaultIdType, RetainedEarningsViewModel> _table = null!;
    
    /// <summary>
    /// Search filter for fiscal year.
    /// </summary>
    private int? SearchFiscalYear { get; set; }

    /// <summary>
    /// Search filter for status.
    /// </summary>
    private string? SearchStatus { get; set; }

    /// <summary>
    /// Search filter to show only open years.
    /// </summary>
    private bool SearchOnlyOpen { get; set; }

    /// <summary>
    /// Gets the status color based on retained earnings status.
    /// </summary>
    private static Color GetStatusColor(string? status) => status switch
    {
        "Closed" => Color.Success,
        "Open" => Color.Info,
        "Locked" => Color.Error,
        _ => Color.Default
    };

    /// <summary>
    /// Initializes the component and sets up the entity table context.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<RetainedEarningsResponse, DefaultIdType, RetainedEarningsViewModel>(
            entityName: "Retained Earnings",
            entityNamePlural: "Retained Earnings",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<RetainedEarningsResponse>(re => re.FiscalYear, "Fiscal Year", "FiscalYear"),
                new EntityField<RetainedEarningsResponse>(re => re.BeginningBalance, "Opening Balance", "BeginningBalance", Type: typeof(decimal)),
                new EntityField<RetainedEarningsResponse>(re => re.NetIncome, "Net Income", "NetIncome", Type: typeof(decimal)),
                new EntityField<RetainedEarningsResponse>(re => re.Dividends, "Distributions", "Dividends", Type: typeof(decimal)),
                new EntityField<RetainedEarningsResponse>(re => re.EndingBalance, "Closing Balance", "EndingBalance", Type: typeof(decimal)),
                new EntityField<RetainedEarningsResponse>(re => re.Status, "Status", "Status"),
            ],
            enableAdvancedSearch: true,
            idFunc: re => re.Id,
            searchFunc: async filter =>
            {
                var request = new SearchRetainedEarningsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    FiscalYear = SearchFiscalYear,
                    Status = SearchStatus,
                    IsClosed = SearchOnlyOpen ? false : null
                };

                var result = await Client.RetainedEarningsSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<RetainedEarningsResponse>>();
            },
            createFunc: async viewModel =>
            {
                var command = new RetainedEarningsCreateCommand
                {
                    FiscalYear = viewModel.FiscalYear,
                    OpeningBalance = viewModel.OpeningBalance,
                    FiscalYearStartDate = viewModel.FiscalYearStartDate!.Value,
                    FiscalYearEndDate = viewModel.FiscalYearEndDate!.Value,
                    RetainedEarningsAccountId = viewModel.RetainedEarningsAccountId,
                    Description = viewModel.Description,
                    Notes = viewModel.Notes
                };

                await Client.RetainedEarningsCreateEndpointAsync("1", command);
                Snackbar.Add($"Retained Earnings for FY{viewModel.FiscalYear} created successfully", Severity.Success);
            },
            updateFunc: null, // Retained earnings don't support direct updates
            deleteFunc: null); // Delete not yet implemented

        base.OnInitialized();
    }

    /// <summary>
    /// Opens the details dialog to view complete retained earnings information.
    /// </summary>
    private async Task OnViewDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(RetainedEarningsDetailsDialog.RetainedEarningsId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        await DialogService.ShowAsync<RetainedEarningsDetailsDialog>(
            "Retained Earnings Details",
            parameters,
            options);
    }

    /// <summary>
    /// Opens the dialog to update net income.
    /// </summary>
    private async Task OnUpdateNetIncome(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(RetainedEarningsUpdateNetIncomeDialog.RetainedEarningsId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<RetainedEarningsUpdateNetIncomeDialog>(
            "Update Net Income",
            parameters,
            options);

        var result = await dialog.Result;
        if (!result.Canceled && _table is not null)
        {

            await _table.ReloadDataAsync();

        }
    }

    /// <summary>
    /// Opens the dialog to record a distribution.
    /// </summary>
    private async Task OnRecordDistribution(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(RetainedEarningsDistributionDialog.RetainedEarningsId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<RetainedEarningsDistributionDialog>(
            "Record Distribution",
            parameters,
            options);

        var result = await dialog.Result;
        if (!result.Canceled && _table is not null)
        {

            await _table.ReloadDataAsync();

        }
    }

    /// <summary>
    /// Closes the retained earnings year.
    /// </summary>
    private async Task OnClose(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox(
            "Close Fiscal Year",
            "Are you sure you want to close this fiscal year? This will lock the retained earnings record and prevent further modifications.",
            yesText: "Close Year",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                var command = new CloseRetainedEarningsCommand();

                await Client.RetainedEarningsCloseEndpointAsync("1", id, command);
                Snackbar.Add("Fiscal year closed successfully", Severity.Success);
                if (_table is not null)
                    await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error closing fiscal year: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Reopens a closed retained earnings year.
    /// </summary>
    private async Task OnReopen(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox(
            "Reopen Fiscal Year",
            "Are you sure you want to reopen this fiscal year? This will allow modifications to the retained earnings record.",
            yesText: "Reopen Year",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                var command = new ReopenRetainedEarningsCommand
                {
                    Reason = "Manual reopen for corrections"
                };

                await Client.RetainedEarningsReopenEndpointAsync("1", id, command);
                Snackbar.Add("Fiscal year reopened successfully", Severity.Success);
                if (_table is not null)
                    await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error reopening fiscal year: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Opens the statement of retained earnings dialog.
    /// </summary>
    private async Task OnViewStatement(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(RetainedEarningsStatementDialog.RetainedEarningsId), id }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        await DialogService.ShowAsync<RetainedEarningsStatementDialog>(
            "Statement of Retained Earnings",
            parameters,
            options);
    }

    /// <summary>
    /// Show retained earnings help dialog.
    /// </summary>
    private async Task ShowRetainedEarningsHelp()
    {
        await DialogService.ShowAsync<RetainedEarningsHelpDialog>("Retained Earnings Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

