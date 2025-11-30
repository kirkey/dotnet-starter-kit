namespace FSH.Starter.Blazor.Client.Pages.Store.SalesImports;

/// <summary>
/// Dialog to display detailed information about a sales import including all items.
/// </summary>
public partial class SalesImportDetailsDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public DefaultIdType ImportId { get; set; }

    private SalesImportDetailResponse? _import;
    private bool _loading = true;
    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        // Load elevation preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        try
        {
            _import = await Client.GetSalesImportEndpointAsync("1", ImportId);
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

