namespace FSH.Starter.Blazor.Client.Pages.Accounting.Banks;

/// <summary>
/// Help dialog providing comprehensive guidance on banks management.
/// </summary>
public partial class BanksHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

