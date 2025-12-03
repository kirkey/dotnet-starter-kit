namespace FSH.Starter.Blazor.Client.Pages.Accounting.Budgets;

public partial class Budgets
{
    protected EntityServerTableContext<BudgetResponse, DefaultIdType, BudgetViewModel> Context { get; set; } = null!;

    private EntityTable<BudgetResponse, DefaultIdType, BudgetViewModel> _table = null!;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    private string? BudgetName { get; set; }
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

    private string? _searchBudgetType;
    private string? SearchBudgetType
    {
        get => _searchBudgetType;
        set
        {
            _searchBudgetType = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private int? _searchFiscalYear;
    private int? SearchFiscalYear
    {
        get => _searchFiscalYear;
        set
        {
            _searchFiscalYear = value;
            _ = _table.ReloadDataAsync();
        }
    }

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

        Context = new EntityServerTableContext<BudgetResponse, DefaultIdType, BudgetViewModel>(
            entityName: "Budget",
            entityNamePlural: "Budgets",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<BudgetResponse>(response => response.PeriodName, "Period", "PeriodName"),
                new EntityField<BudgetResponse>(response => response.Name, "Name", "Name"),
                new EntityField<BudgetResponse>(response => response.FiscalYear, "Fiscal Year", "FiscalYear"),
                new EntityField<BudgetResponse>(response => response.BudgetType, "Type", "BudgetType"),
                new EntityField<BudgetResponse>(response => response.Status, "Status", "Status"),
                new EntityField<BudgetResponse>(response => response.TotalBudgetedAmount, "Total Budgeted", "TotalBudgetedAmount", typeof(decimal)),
                new EntityField<BudgetResponse>(response => response.TotalActualAmount, "Total Actual", "TotalActualAmount", typeof(decimal)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchBudgetsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    Status = SearchStatus,
                    FiscalYear = SearchFiscalYear
                };
                var result = await Client.BudgetSearchEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<BudgetResponse>>();
            },
            createFunc: async period =>
            {
                await Client.BudgetCreateEndpointAsync("1", period.Adapt<CreateBudgetCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, period) =>
            {
                await Client.BudgetUpdateEndpointAsync("1", id, period.Adapt<UpdateBudgetCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.BudgetDeleteEndpointAsync("1", id).ConfigureAwait(false));

        await Task.CompletedTask;
    }

    private async Task OnApproveBudget(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox("Approve Budget", "Are you sure you want to approve this budget?", yesText: "Approve", cancelText: "Cancel");
        if (confirmed == true)
        {
            try
            {
                var command = new ApproveBudgetCommand();
                await Client.BudgetApproveEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Budget approved successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to approve budget: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnCloseBudget(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox("Close Budget", "Are you sure you want to close this budget? This action cannot be undone.", yesText: "Close", cancelText: "Cancel");
        if (confirmed == true)
        {
            try
            {
                var command = new CloseBudgetCommand();
                await Client.BudgetCloseEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Budget closed successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to close budget: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Show budgets help dialog.
    /// </summary>
    private async Task ShowBudgetsHelp()
    {
        await DialogService.ShowAsync<BudgetsHelpDialog>("Budgets Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

public class BudgetViewModel
{
    // Core identifiers
    public DefaultIdType Id { get; set; }
    public string? Name { get; set; }

    // Fields from CreateBudgetRequest / BudgetDto
    public DefaultIdType? PeriodId { get; set; }
    public string? PeriodName { get; set; }
    public int FiscalYear { get; set; }
    public string? BudgetType { get; set; }

    // Optional status used in UpdateBudgetRequest
    public string? Status { get; set; }

    // Budget totals from BudgetDto
    public decimal TotalBudgetedAmount { get; set; }
    public decimal TotalActualAmount { get; set; }

    // Approval info from BudgetDto
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }

    // Common descriptive fields
    public string? Description { get; set; }
    public string? Notes { get; set; }
}
