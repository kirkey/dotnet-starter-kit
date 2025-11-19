namespace FSH.Starter.Blazor.Client.Pages.Store.SerialNumbers;

/// <summary>
/// Help dialog providing comprehensive guidance on serial number management.
/// </summary>
public partial class SerialNumbersHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

