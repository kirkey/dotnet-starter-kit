namespace FSH.Starter.Blazor.Client.Pages.Accounting.DepreciationMethods;

/// <summary>
/// Help dialog providing comprehensive guidance on depreciation methods.
/// </summary>
public partial class DepreciationMethodsHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

