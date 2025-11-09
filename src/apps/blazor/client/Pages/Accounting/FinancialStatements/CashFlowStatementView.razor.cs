namespace FSH.Starter.Blazor.Client.Pages.Accounting.FinancialStatements;

public partial class CashFlowStatementView
{
    private DateTime? _startDate = DateTime.Today.AddMonths(-1);
    private DateTime? _endDate = DateTime.Today;
    private string _method = "Indirect";
    private bool _loading = false;
    private CashFlowStatementDto? _cashFlowStatement;

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
            var request = new GenerateCashFlowStatementQuery
            {
                StartDate = _startDate.Value,
                EndDate = _endDate.Value,
                Method = _method
            };

            _cashFlowStatement = await Client.GenerateCashFlowStatementEndpointAsync("1", request);
            Snackbar.Add("Cash Flow Statement generated successfully", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error generating Cash Flow Statement: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }
}

