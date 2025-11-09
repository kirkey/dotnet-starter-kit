namespace FSH.Starter.Blazor.Client.Pages.Accounting.FixedAssets;

public partial class FixedAssetDepreciateDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType FixedAssetId { get; set; }
    
    private decimal _depreciationAmount;
    private DateTime? _depreciationDate = DateTime.Today;
    private string? _method;

    private async Task Record()
    {
        if (_depreciationAmount <= 0 || _depreciationDate == null)
        {
            Snackbar.Add("Depreciation amount and date are required", Severity.Warning);
            return;
        }

        try
        {
            var command = new DepreciateFixedAssetCommand
            {
                FixedAssetId = FixedAssetId,
                DepreciationAmount = _depreciationAmount,
                DepreciationDate = _depreciationDate.Value,
                DepreciationMethod = _method
            };
            await Client.FixedAssetDepreciateEndpointAsync("1", FixedAssetId, command);
            Snackbar.Add("Depreciation recorded successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error recording depreciation: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

