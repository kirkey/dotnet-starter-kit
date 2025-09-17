namespace FSH.Starter.Blazor.Client.Components.Common;

public static class DialogServiceExtensions
{
    public static Task<DialogResult> ShowModalAsync<TDialog>(this IDialogService dialogService, DialogParameters parameters)
        where TDialog : ComponentBase =>
        dialogService.ShowModal<TDialog>(parameters).Result!;

    public static IDialogReference ShowModal<TDialog>(this IDialogService dialogService, DialogParameters parameters)
        where TDialog : ComponentBase
    {
        var options = new DialogOptions
        {
            BackdropClick = false,
            CloseButton = true, 
            FullWidth = true, 
            MaxWidth = MaxWidth.Large, 
        };

        return dialogService.Show<TDialog>(string.Empty, parameters, options);
    }
}
