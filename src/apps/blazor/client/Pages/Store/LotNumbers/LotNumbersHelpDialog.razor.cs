namespace FSH.Starter.Blazor.Client.Pages.Store.LotNumbers;

/// <summary>
/// Help dialog providing comprehensive guidance on lot number management.
/// </summary>
public partial class LotNumbersHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

