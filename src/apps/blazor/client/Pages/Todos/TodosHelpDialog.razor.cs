namespace FSH.Starter.Blazor.Client.Pages.Todos;

public partial class TodosHelpDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private void Close() => MudDialog.Close();
}
