namespace FSH.Starter.Blazor.Client.Pages.Accounting.Invoices;

/// <summary>
/// Dialog component for displaying detailed invoice information.
/// </summary>
public partial class InvoiceDetailsDialog
{
    /// <summary>
    /// Invoice identifier to load details for.
    /// </summary>
    [Parameter]
    public DefaultIdType InvoiceId { get; set; }

    /// <summary>
    /// Dialog instance reference.
    /// </summary>
    [CascadingParameter]
    public IMudDialogInstance MudDialog { get; set; } = default!;

    private InvoiceResponse? _invoice;
    private List<InvoiceLineItemViewModel> _lineItems = [];
    private bool _isLoading = true;

    /// <summary>
    /// Gets the status color based on invoice status.
    /// </summary>
    private static Color GetStatusColor(string? status) => status switch
    {
        "Draft" => Color.Default,
        "Sent" => Color.Info,
        "Paid" => Color.Success,
        "Overdue" => Color.Error,
        "Cancelled" => Color.Warning,
        "Voided" => Color.Dark,
        _ => Color.Default
    };

    /// <summary>
    /// Loads invoice details on component initialization.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadInvoiceDetails();
    }

    /// <summary>
    /// Loads the invoice details from the API.
    /// </summary>
    private async Task LoadInvoiceDetails()
    {
        try
        {
            _isLoading = true;
            _invoice = await Client.GetInvoiceEndpointAsync("1", InvoiceId);
            
            // Load line items if any exist
            // Note: This assumes line items are included in the response or need separate endpoint
            _lineItems = []; // TODO: Load from API when endpoint is available
            
            _isLoading = false;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading invoice details: {ex.Message}", Severity.Error);
            _isLoading = false;
        }
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void Cancel()
    {
        MudDialog.Cancel();
    }

    /// <summary>
    /// Sends the invoice.
    /// </summary>
    private async Task OnSendInvoice()
    {
        try
        {
            await Client.SendInvoiceEndpointAsync("1", InvoiceId);
            Snackbar.Add("Invoice sent successfully", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error sending invoice: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Prints the invoice.
    /// </summary>
    private void OnPrint()
    {
        Snackbar.Add("Print functionality not yet implemented", Severity.Info);
    }
}

