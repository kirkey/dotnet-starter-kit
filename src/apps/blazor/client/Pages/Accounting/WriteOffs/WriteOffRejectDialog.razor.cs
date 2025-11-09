namespace FSH.Starter.Blazor.Client.Pages.Accounting.WriteOffs;

public partial class WriteOffRejectDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType WriteOffId { get; set; }
    
    private string? _reason;
    private string UserId => "system"; // TODO: Get from current user context

    private async Task Reject()
    {
        try
        {
            var cmd = new RejectWriteOffCommand
            {
                Id = WriteOffId,
                RejectedBy = UserId,
                Reason = _reason
            };
            await Client.WriteOffRejectEndpointAsync("1", WriteOffId, cmd);
            Snackbar.Add("Write-off rejected successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error rejecting write-off: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

