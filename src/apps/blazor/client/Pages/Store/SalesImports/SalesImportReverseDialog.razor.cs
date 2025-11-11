namespace FSH.Starter.Blazor.Client.Pages.Store.SalesImports;

/// <summary>
/// Dialog for reversing a sales import and undoing inventory adjustments.
/// </summary>
public partial class SalesImportReverseDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType Id { get; set; }

    private string _reason = string.Empty;
    private bool _processing;

    private async Task ReverseImportAsync()
    {
        if (string.IsNullOrWhiteSpace(_reason))
        {
            Snackbar.Add("Please provide a reason for reversal", Severity.Warning);
            return;
        }

        _processing = true;
        try
        {
            var command = new ReverseSalesImportCommand
            {
                Reason = _reason
            };

            await Client.ReverseSalesImportEndpointAsync("1", Id, command);
            Snackbar.Add("Sales import reversed successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error reversing import: {ex.Message}", Severity.Error);
        }
        finally
        {
            _processing = false;
        }
    }
}

