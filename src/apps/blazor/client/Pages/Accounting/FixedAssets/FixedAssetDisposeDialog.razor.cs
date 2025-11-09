namespace FSH.Starter.Blazor.Client.Pages.Accounting.FixedAssets;

public partial class FixedAssetDisposeDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType FixedAssetId { get; set; }
    
    private DateTime? _disposalDate = DateTime.Today;
    private decimal? _disposalAmount;
    private string? _disposalReason;

    private async Task Dispose()
    {
        if (_disposalDate == null)
        {
            Snackbar.Add("Disposal date is required", Severity.Warning);
            return;
        }

        try
        {
            var command = new DisposeFixedAssetCommand
            {
                Id = FixedAssetId,
                DisposalDate = _disposalDate.Value,
                DisposalAmount = _disposalAmount,
                DisposalReason = _disposalReason
            };
            await Client.FixedAssetDisposeEndpointAsync("1", FixedAssetId, command);
            Snackbar.Add("Fixed asset disposed successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error disposing fixed asset: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

