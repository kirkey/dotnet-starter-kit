namespace FSH.Starter.Blazor.Client.Pages.Accounting.TaxCodes;

/// <summary>
/// Help dialog providing comprehensive guidance on tax codes.
/// </summary>
public partial class TaxCodesHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

