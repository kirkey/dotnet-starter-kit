namespace FSH.Starter.Blazor.Client.Pages.Accounting.AccountingPeriods;

public partial class AccountingPeriods
{
    protected EntityServerTableContext<AccountingPeriodResponse, DefaultIdType, AccountingPeriodViewModel> Context { get; set; } = null!;

    private EntityTable<AccountingPeriodResponse, DefaultIdType, AccountingPeriodViewModel> _table = null!;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

    // Advanced search filters
    private bool? _searchClosedOnly;
    private bool? SearchClosedOnly
    {
        get => _searchClosedOnly;
        set
        {
            _searchClosedOnly = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchPeriodType;
    private string? SearchPeriodType
    {
        get => _searchPeriodType;
        set
        {
            _searchPeriodType = value;
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

        Context = new EntityServerTableContext<AccountingPeriodResponse, DefaultIdType, AccountingPeriodViewModel>(
            entityName: "Accounting Period",
            entityNamePlural: "Accounting Periods",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<AccountingPeriodResponse>(response => response.Name, "Name", "Name"),
                new EntityField<AccountingPeriodResponse>(response => response.StartDate, "Start Date", "StartDate", typeof(DateOnly)),
                new EntityField<AccountingPeriodResponse>(response => response.EndDate, "End Date", "EndDate", typeof(DateOnly)),
                new EntityField<AccountingPeriodResponse>(response => response.IsClosed, "Closed", "IsClosed", typeof(bool)),
                new EntityField<AccountingPeriodResponse>(response => response.IsAdjustmentPeriod, "Adjustment Period", "IsAdjustmentPeriod", typeof(bool)),
                new EntityField<AccountingPeriodResponse>(response => response.FiscalYear, "Fiscal Year", "FiscalYear"),
                new EntityField<AccountingPeriodResponse>(response => response.PeriodType, "Period Type", "PeriodType"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchAccountingPeriodsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.AccountingPeriodSearchEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<AccountingPeriodResponse>>();
            },
            createFunc: async period =>
            {
                await Client.AccountingPeriodCreateEndpointAsync("1", period.Adapt<CreateAccountingPeriodCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, period) =>
            {
                await Client.AccountingPeriodUpdateEndpointAsync("1", id, period.Adapt<UpdateAccountingPeriodCommand>()).ConfigureAwait(false);
            },
            hasExtraActionsFunc: () => true);

        await Task.CompletedTask;
    }

    private async Task OnClosePeriod(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox("Close Period", "Are you sure you want to close this accounting period? This will prevent further transactions.", yesText: "Close", cancelText: "Cancel");
        if (confirmed == true)
        {
            try
            {
                await Client.AccountingPeriodCloseEndpointAsync("1", id, new()).ConfigureAwait(false);
                Snackbar.Add("Accounting period closed successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to close period: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnReopenPeriod(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox("Reopen Period", "Are you sure you want to reopen this accounting period? This will allow transactions to be posted again.", yesText: "Reopen", cancelText: "Cancel");
        if (confirmed == true)
        {
            try
            {
                await Client.AccountingPeriodReopenEndpointAsync("1", id, new()).ConfigureAwait(false);
                Snackbar.Add("Accounting period reopened successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to reopen period: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ShowAccountingPeriodsHelp()
    {
        await DialogService.ShowAsync<AccountingPeriodsHelpDialog>("Accounting Periods Help", new DialogParameters(), _helpDialogOptions);
    }
}

public class AccountingPeriodViewModel : UpdateAccountingPeriodCommand
{
    // Properties will be inherited from UpdateAccountingPeriodCommand
    // This class serves as the view model for the entity table
}
