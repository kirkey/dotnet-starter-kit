namespace FSH.Starter.Blazor.Client.Pages.Store.SalesImports;

/// <summary>
/// Dialog to display detailed information about a sales import including all items.
/// </summary>
public partial class SalesImportDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType Id { get; set; }

    private SalesImportDetailResponse? _import;
    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _import = await Client.GetSalesImportEndpointAsync("1", Id);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading import details: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private static Color GetStatusColor(string status) => status switch
    {
        "PENDING" => Color.Default,
        "PROCESSING" => Color.Info,
        "COMPLETED" => Color.Success,
        "FAILED" => Color.Error,
        _ => Color.Default
    };
}

