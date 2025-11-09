namespace FSH.Starter.Blazor.Client.Pages.Accounting.RetainedEarnings;

/// <summary>
/// Dialog for displaying the Statement of Retained Earnings.
/// </summary>
public partial class RetainedEarningsStatementDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public DefaultIdType RetainedEarningsId { get; set; }

    private RetainedEarningsDetailsResponse? _retainedEarnings;
    private bool _loading;

    /// <summary>
    /// Initializes the dialog and loads data.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    /// <summary>
    /// Loads the retained earnings data.
    /// </summary>
    private async Task LoadDataAsync()
    {
        _loading = true;
        StateHasChanged();

        try
        {
            _retainedEarnings = await Client.RetainedEarningsGetEndpointAsync("1", RetainedEarningsId);
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Calculates the subtotal before distributions.
    /// </summary>
    private decimal GetSubtotal()
    {
        if (_retainedEarnings == null) return 0;
        
        return _retainedEarnings.BeginningBalance 
               + _retainedEarnings.NetIncome 
               + _retainedEarnings.CapitalContributions 
               + _retainedEarnings.OtherEquityChanges;
    }

    /// <summary>
    /// Gets the color style for income display (green for positive, red for negative).
    /// </summary>
    private static string GetIncomeColor(decimal amount)
    {
        return amount >= 0 
            ? "color: var(--mud-palette-success);" 
            : "color: var(--mud-palette-error);";
    }

    /// <summary>
    /// Prints the statement (placeholder for print functionality).
    /// </summary>
    private void Print()
    {
        Snackbar.Add("Print functionality coming soon", Severity.Info);
        // TODO: Implement print functionality
        // Could use JavaScript interop to trigger browser print dialog
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void Close()
    {
        MudDialog.Close();
    }
}
