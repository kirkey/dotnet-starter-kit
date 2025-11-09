namespace FSH.Starter.Blazor.Client.Pages.Accounting.FixedAssets;

public partial class FixedAssetRejectDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType FixedAssetId { get; set; }
    
    private string? _reason;
    private string UserId => "system"; // TODO: Get from current user context

    private async Task Reject()
    {
        try
        {
            var cmd = new RejectFixedAssetCommand
            {
                FixedAssetId = FixedAssetId,
                RejectedBy = UserId,
                Reason = _reason
            };
            await Client.FixedAssetRejectEndpointAsync("1", FixedAssetId, cmd);
            Snackbar.Add("Fixed asset rejected successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error rejecting fixed asset: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

