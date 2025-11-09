namespace FSH.Starter.Blazor.Client.Pages.Accounting.DeferredRevenue;

public partial class DeferredRevenueRecognizeDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType DeferredRevenueId { get; set; }
    [Parameter] public string DeferredRevenueNumber { get; set; } = string.Empty;
    
    private DateTime? _recognizedDate = DateTime.Today;

    private async Task Recognize()
    {
        if (!_recognizedDate.HasValue)
        {
            Snackbar.Add("Recognition date is required", Severity.Warning);
            return;
        }

        try
        {
            var cmd = new RecognizeDeferredRevenueCommand
            {
                Id = DeferredRevenueId,
                RecognizedDate = _recognizedDate.Value
            };
            await Client.DeferredRevenueRecognizeEndpointAsync("1", DeferredRevenueId, cmd);
            Snackbar.Add("Deferred revenue recognized successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error recognizing revenue: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

