
namespace FSH.Starter.Blazor.Client.Pages.Accounting.WriteOffs;

public partial class WriteOffReverseDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType WriteOffId { get; set; }
    
    private string? _reason;

    private async Task Reverse()
    {
        try
        {
            var cmd = new ReverseWriteOffCommand
            {
                Id = WriteOffId,
                Reason = _reason
            };
            await Client.WriteOffReverseEndpointAsync("1", WriteOffId, cmd);
            Snackbar.Add("Write-off reversed successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error reversing write-off: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}
