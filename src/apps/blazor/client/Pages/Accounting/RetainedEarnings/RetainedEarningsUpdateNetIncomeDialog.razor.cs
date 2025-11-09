namespace FSH.Starter.Blazor.Client.Pages.Accounting.RetainedEarnings;

/// <summary>
/// Dialog for updating net income for retained earnings.
/// </summary>
public partial class RetainedEarningsUpdateNetIncomeDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public DefaultIdType RetainedEarningsId { get; set; }

    private int _fiscalYear;
    private decimal _netIncome;
    private string? _notes;
    private bool _loading;

    /// <summary>
    /// Initializes the dialog and loads data.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    /// <summary>
    /// Loads the current retained earnings data.
    /// </summary>
    private async Task LoadDataAsync()
    {
        _loading = true;
        StateHasChanged();

        try
        {
            var data = await Client.RetainedEarningsGetEndpointAsync("1", RetainedEarningsId);
            _fiscalYear = data.FiscalYear;
            _netIncome = data.NetIncome;
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Submits the net income update.
    /// </summary>
    private async Task Submit()
    {
        _loading = true;
        StateHasChanged();

        try
        {
            var command = new UpdateNetIncomeCommand
            {
                NetIncome = _netIncome
            };

            await Client.RetainedEarningsUpdateNetIncomeEndpointAsync("1", RetainedEarningsId, command);
            Snackbar.Add("Net income updated successfully", Severity.Success);
            
            MudDialog.Close();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error updating net income: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Cancels the operation.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Close();
    }
}
