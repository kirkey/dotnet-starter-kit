
namespace FSH.Starter.Blazor.Client.Pages.Accounting.WriteOffs;

public partial class WriteOffPostDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType WriteOffId { get; set; }
    
    private DefaultIdType _journalEntryId = DefaultIdType.Empty;

    private async Task Post()
    {
        if (_journalEntryId == DefaultIdType.Empty)
        {
            Snackbar.Add("Journal Entry ID is required", Severity.Warning);
            return;
        }

        try
        {
            var cmd = new PostWriteOffCommand
            {
                Id = WriteOffId,
                JournalEntryId = _journalEntryId
            };
            await Client.WriteOffPostEndpointAsync("1", WriteOffId, cmd);
            Snackbar.Add("Write-off posted successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error posting write-off: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}
