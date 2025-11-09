namespace FSH.Starter.Blazor.Client.Pages.Accounting.FinancialStatements;

public partial class FinancialStatements
{
    private int _activeTab = 0;
    private BalanceSheetView? _balanceSheetView;
    private IncomeStatementView? _incomeStatementView;
    private CashFlowStatementView? _cashFlowStatementView;

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
}

