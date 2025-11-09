namespace FSH.Starter.Blazor.Client.Pages.Accounting.DepreciationMethods;

public partial class DepreciationMethodDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DepreciationMethodResponse Method { get; set; } = default!;
    
    private DepreciationMethodResponse? _method;
    private bool _loading = false;

    protected override Task OnInitializedAsync()
    {
        _method = Method;
        return Task.CompletedTask;
    }

    private void Cancel() => MudDialog?.Close();
}

