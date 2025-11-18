namespace FSH.Starter.Blazor.Client.Pages.Accounting.Budgets;

/// <summary>
/// Help dialog providing comprehensive guidance on budget management.
/// </summary>
public partial class BudgetsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

