namespace FSH.Starter.Blazor.Client.Pages.Accounting.Bills;

/// <summary>
/// Help dialog providing comprehensive guidance on bill management and accounts payable.
/// </summary>
public partial class BillsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

