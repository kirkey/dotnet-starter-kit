namespace FSH.Starter.Blazor.Client.Pages.Accounting.DeferredRevenue;

public partial class DeferredRevenueDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DeferredRevenueResponse Revenue { get; set; } = default!;

    private void Cancel() => MudDialog?.Close();
}

