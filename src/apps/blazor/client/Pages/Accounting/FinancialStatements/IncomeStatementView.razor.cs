namespace FSH.Starter.Blazor.Client.Pages.Accounting.FinancialStatements;

public partial class IncomeStatementView
{
    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }
    }

    private DateTime? _startDate = DateTime.Today.AddMonths(-1);
    private DateTime? _endDate = DateTime.Today;
    private string _reportFormat = "Standard";
    private bool _loading = false;
    private IncomeStatementDto? _incomeStatement;

    public async Task Generate()
    {
        if (!_startDate.HasValue || !_endDate.HasValue)
        {
            Snackbar.Add("Please select start and end dates", Severity.Warning);
            return;
        }

        if (_startDate.Value >= _endDate.Value)
        {
            Snackbar.Add("Start date must be before end date", Severity.Warning);
            return;
        }

        _loading = true;
        try
        {
            var request = new GenerateIncomeStatementQuery
            {
                StartDate = _startDate.Value,
                EndDate = _endDate.Value,
                ReportFormat = _reportFormat
            };

            _incomeStatement = await Client.GenerateIncomeStatementEndpointAsync("1", request);
            Snackbar.Add("Income Statement generated successfully", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error generating Income Statement: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }
}

