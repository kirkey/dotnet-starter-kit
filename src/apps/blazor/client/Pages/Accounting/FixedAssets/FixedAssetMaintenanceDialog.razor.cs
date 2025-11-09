namespace FSH.Starter.Blazor.Client.Pages.Accounting.FixedAssets;

public partial class FixedAssetMaintenanceDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType FixedAssetId { get; set; }
    
    private DateTime? _lastMaintenanceDate;
    private DateTime? _nextMaintenanceDate;

    private async Task Update()
    {
        try
        {
            var command = new UpdateMaintenanceCommand
            {
                Id = FixedAssetId,
                LastMaintenanceDate = _lastMaintenanceDate,
                NextMaintenanceDate = _nextMaintenanceDate
            };
            await Client.FixedAssetUpdateMaintenanceEndpointAsync("1", FixedAssetId, command);
            Snackbar.Add("Maintenance schedule updated successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error updating maintenance: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel() => MudDialog?.Close(DialogResult.Cancel());
}

