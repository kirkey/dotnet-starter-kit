namespace FSH.Starter.Blazor.Client.Pages.Accounting.FixedAssets;

public partial class FixedAssetDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType FixedAssetId { get; set; }
    
    private FixedAssetResponse? _asset;
    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        try
        {
            _asset = await Client.FixedAssetGetEndpointAsync("1", FixedAssetId);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading fixed asset: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private void Cancel() => MudDialog?.Close();
}

