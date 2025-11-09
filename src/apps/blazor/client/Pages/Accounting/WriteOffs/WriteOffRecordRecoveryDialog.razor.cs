
namespace FSH.Starter.Blazor.Client.Pages.Accounting.WriteOffs;

public partial class WriteOffRecordRecoveryDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType WriteOffId { get; set; }
    
    private decimal _recoveryAmount;
    private DefaultIdType? _journalEntryId;

    private async Task Record()
    {
        if (_recoveryAmount <= 0)
        {
            Snackbar.Add("Recovery amount must be greater than zero", Severity.Warning);
            return;
        }

        try
        {
            var cmd = new RecordRecoveryCommand
            {
                Id = WriteOffId,
                RecoveryAmount = _recoveryAmount,
                RecoveryJournalEntryId = _journalEntryId
            };
            await Client.WriteOffRecordRecoveryEndpointAsync("1", WriteOffId, cmd);
            Snackbar.Add("Recovery recorded successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error recording recovery: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}
