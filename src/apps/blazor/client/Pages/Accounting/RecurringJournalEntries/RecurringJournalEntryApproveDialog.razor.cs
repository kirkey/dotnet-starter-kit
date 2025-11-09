namespace FSH.Starter.Blazor.Client.Pages.Accounting.RecurringJournalEntries;

public partial class RecurringJournalEntryApproveDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType TemplateId { get; set; }
    [Parameter] public string TemplateCode { get; set; } = string.Empty;

    private string? _approverNotes;

    private async Task Approve()
    {
        try
        {
            var cmd = new ApproveRecurringJournalEntryCommand
            {
                Id = TemplateId,
                ApproverNotes = _approverNotes
            };
            await Client.RecurringJournalEntryApproveEndpointAsync("1", TemplateId, cmd);
            Snackbar.Add("Template approved successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error approving template: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

