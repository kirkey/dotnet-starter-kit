namespace FSH.Starter.Blazor.Client.Pages.Accounting.RecurringJournalEntries;

public partial class RecurringJournalEntrySuspendDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType TemplateId { get; set; }
    [Parameter] public string TemplateCode { get; set; } = string.Empty;

    private string? _reason;

    private async Task Suspend()
    {
        if (string.IsNullOrWhiteSpace(_reason))
        {
            Snackbar.Add("Reason is required", Severity.Warning);
            return;
        }

        try
        {
            var cmd = new SuspendRecurringJournalEntryCommand
            {
                Id = TemplateId,
                Reason = _reason
            };
            await Client.RecurringJournalEntrySuspendEndpointAsync("1", TemplateId, cmd);
            Snackbar.Add("Template suspended successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error suspending template: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

