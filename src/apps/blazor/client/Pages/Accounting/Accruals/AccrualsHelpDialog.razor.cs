namespace FSH.Starter.Blazor.Client.Pages.Accounting.Accruals;

/// <summary>
/// Help dialog providing comprehensive guidance on accruals management.
/// </summary>
public partial class AccrualsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

