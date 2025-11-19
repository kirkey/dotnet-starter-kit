namespace FSH.Starter.Blazor.Client.Pages.Store.Categories;

/// <summary>
/// Help dialog providing comprehensive guidance on product category management.
/// </summary>
public partial class CategoriesHelpDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

