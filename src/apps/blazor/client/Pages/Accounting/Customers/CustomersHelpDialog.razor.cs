namespace FSH.Starter.Blazor.Client.Pages.Accounting.Customers;

/// <summary>
/// Help dialog providing comprehensive guidance on customer management.
/// </summary>
public partial class CustomersHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

