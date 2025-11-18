namespace FSH.Starter.Blazor.Client.Pages.Accounting.FiscalPeriodClose;

/// <summary>
/// Help dialog providing comprehensive guidance on fiscal period close process.
/// </summary>
public partial class FiscalPeriodCloseHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

