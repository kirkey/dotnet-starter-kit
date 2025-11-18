namespace FSH.Starter.Blazor.Client.Pages.Accounting.PrepaidExpenses;

/// <summary>
/// Help dialog providing comprehensive guidance on prepaid expense management.
/// </summary>
public partial class PrepaidExpensesHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

