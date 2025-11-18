namespace FSH.Starter.Blazor.Client.Pages.Accounting.FinancialStatements;

/// <summary>
/// Help dialog providing comprehensive guidance on financial statements.
/// </summary>
public partial class FinancialStatementsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

