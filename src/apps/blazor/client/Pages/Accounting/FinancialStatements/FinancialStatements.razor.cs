namespace FSH.Starter.Blazor.Client.Pages.Accounting.FinancialStatements;

public partial class FinancialStatements
{
    

    private int _activeTab = 0;
    private BalanceSheetView? _balanceSheetView;
    private IncomeStatementView? _incomeStatementView;
    private CashFlowStatementView? _cashFlowStatementView;

    private ClientPreference _preference = new();

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
    }

    private async Task RefreshCurrentStatement()
    {
        switch (_activeTab)
        {
            case 0:
                if (_balanceSheetView is not null)
                    await _balanceSheetView.Generate();
                break;
            case 1:
                if (_incomeStatementView is not null)
                    await _incomeStatementView.Generate();
                break;
            case 2:
                if (_cashFlowStatementView is not null)
                    await _cashFlowStatementView.Generate();
                break;
        }
    }

    /// <summary>
    /// Show financial statements help dialog.
    /// </summary>
    private async Task ShowFinancialStatementsHelp()
    {
        await DialogService.ShowAsync<FinancialStatementsHelpDialog>("Financial Statements Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
