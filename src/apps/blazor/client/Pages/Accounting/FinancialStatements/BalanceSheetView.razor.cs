namespace FSH.Starter.Blazor.Client.Pages.Accounting.FinancialStatements;

public partial class BalanceSheetView
{
    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }
    }

    private DateTime? _asOfDate = DateTime.Today;
    private string _reportFormat = "Standard";
    private bool _includeComparative = false;
    private DateTime? _comparativeDate;
    private bool _loading = false;
    private BalanceSheetDto? _balanceSheet;

    public async Task Generate()
    {
        if (!_asOfDate.HasValue)
        {
            Snackbar.Add("Please select an as-of date", Severity.Warning);
            return;
        }

        _loading = true;
        try
        {
            var request = new GenerateBalanceSheetQuery
            {
                AsOfDate = _asOfDate.Value,
                ReportFormat = _reportFormat,
                IncludeComparativePeriod = _includeComparative,
                ComparativeAsOfDate = _comparativeDate
            };

            _balanceSheet = await Client.GenerateBalanceSheetEndpointAsync("1", request);
            Snackbar.Add("Balance Sheet generated successfully", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error generating Balance Sheet: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private void Print()
    {
        // TODO: Implement print functionality
        Snackbar.Add("Print functionality coming soon", Severity.Info);
    }

    private void Export()
    {
        // TODO: Implement export functionality
        Snackbar.Add("Export functionality coming soon", Severity.Info);
    }
}

