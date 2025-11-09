namespace FSH.Starter.Blazor.Client.Pages.Accounting.RecurringJournalEntries;

public partial class RecurringJournalEntryGenerateDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType TemplateId { get; set; }
    [Parameter] public string TemplateCode { get; set; } = string.Empty;

    private DateTime? _postingDate = DateTime.Today;
    private string? _memo;

    private async Task Generate()
    {
        if (!_postingDate.HasValue)
        {
            Snackbar.Add("Posting date is required", Severity.Warning);
            return;
        }

        try
        {
            var cmd = new GenerateRecurringJournalEntryCommand
            {
                Id = TemplateId,
                GenerateForDate = _postingDate.Value,
            };
            await Client.RecurringJournalEntryGenerateEndpointAsync("1", TemplateId, cmd);
            Snackbar.Add("Journal entry generated successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error generating entry: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

