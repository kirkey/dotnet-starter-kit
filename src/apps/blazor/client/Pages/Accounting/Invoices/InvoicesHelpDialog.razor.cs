namespace FSH.Starter.Blazor.Client.Pages.Accounting.Invoices;

/// <summary>
/// Help dialog providing comprehensive guidance on invoice management.
/// </summary>
public partial class InvoicesHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

