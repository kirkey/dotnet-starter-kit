namespace FSH.Starter.Blazor.Client.Pages.Accounting.RetainedEarnings;

/// <summary>
/// Dialog for recording distributions from retained earnings.
/// </summary>
public partial class RetainedEarningsDistributionDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public DefaultIdType RetainedEarningsId { get; set; }

    private int _fiscalYear;
    private decimal _availableAmount;
    private decimal _amount;
    private DateTime? _distributionDate = DateTime.UtcNow.Date;
    private string _description = string.Empty;
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
            _availableAmount = data.UnappropriatedAmount;
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Submits the distribution record.
    /// </summary>
    private async Task Submit()
    {
        if (_amount <= 0)
        {
            Snackbar.Add("Distribution amount must be greater than zero", Severity.Warning);
            return;
        }

        if (_amount > _availableAmount)
        {
            Snackbar.Add("Distribution amount exceeds available retained earnings", Severity.Warning);
            return;
        }

        _loading = true;
        StateHasChanged();

        try
        {
            var command = new RecordDistributionCommand
            {
                Amount = _amount,
                DistributionDate = _distributionDate!.Value,
                Description = _description,
                Notes = _notes
            };

            await Client.RetainedEarningsRecordDistributionEndpointAsync("1", RetainedEarningsId, command);
            Snackbar.Add("Distribution recorded successfully", Severity.Success);
            
            MudDialog.Close();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error recording distribution: {ex.Message}", Severity.Error);
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
