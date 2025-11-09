
namespace FSH.Starter.Blazor.Client.Pages.Accounting.WriteOffs;

public partial class WriteOffDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType WriteOffId { get; set; }
    
    private WriteOffResponse? _writeOff;
    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        try
        {
            _writeOff = await Client.WriteOffGetEndpointAsync("1", WriteOffId);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading write-off: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private void Cancel() => MudDialog?.Close();

    private Color GetApprovalColor(string status) => status switch
    {
        "Approved" => Color.Success,
        "Rejected" => Color.Error,
        "Pending" => Color.Warning,
        _ => Color.Default
    };
}

