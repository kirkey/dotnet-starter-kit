namespace FSH.Starter.Blazor.Client.Pages.Accounting.Invoices;

/// <summary>
/// Invoices page for managing customer invoices and accounts receivable.
/// </summary>
public partial class Invoices
{
    /// <summary>
    /// The entity table context for managing invoices with server-side operations.
    /// </summary>
    protected EntityServerTableContext<InvoiceResponse, DefaultIdType, InvoiceViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component for invoices.
    /// </summary>
    private EntityTable<InvoiceResponse, DefaultIdType, InvoiceViewModel> _table = null!;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    /// <summary>
    /// Search filter for invoice number.
    /// </summary>
    private string? InvoiceNumber { get; set; }

    /// <summary>
    /// Search filter for invoice status.
    /// </summary>
    private string? Status { get; set; }

    /// <summary>
    /// Search filter for billing period.
    /// </summary>
    private string? BillingPeriod { get; set; }

    /// <summary>
    /// Search filter for invoice date range start.
    /// </summary>
    private DateTime? InvoiceDateFrom { get; set; }

    /// <summary>
    /// Search filter for invoice date range end.
    /// </summary>
    private DateTime? InvoiceDateTo { get; set; }

    /// <summary>
    /// Search filter for due date range start.
    /// </summary>
    private DateTime? DueDateFrom { get; set; }

    /// <summary>
    /// Search filter for due date range end.
    /// </summary>
    private DateTime? DueDateTo { get; set; }

    /// <summary>
    /// Search filter for minimum amount.
    /// </summary>
    private decimal? MinAmount { get; set; }

    /// <summary>
    /// Search filter for maximum amount.
    /// </summary>
    private decimal? MaxAmount { get; set; }

    /// <summary>
    /// Search filter for invoices with outstanding balance.
    /// </summary>
    private bool HasOutstandingBalance { get; set; }

    /// <summary>
    /// Dialog visibility flag for mark as paid dialog.
    /// </summary>
    private bool _markAsPaidDialogVisible;

    /// <summary>
    /// Dialog visibility flag for apply payment dialog.
    /// </summary>
    private bool _applyPaymentDialogVisible;

    /// <summary>
    /// Dialog visibility flag for cancel invoice dialog.
    /// </summary>
    private bool _cancelDialogVisible;

    /// <summary>
    /// Dialog options for modal dialogs.
    /// </summary>
    private readonly DialogOptions _dialogOptions = new()
    {
        CloseOnEscapeKey = true, 
        MaxWidth = MaxWidth.ExtraLarge, 
        FullWidth = true
    };

    /// <summary>
    /// State for marking an invoice as paid dialog.
    /// </summary>
    private MarkInvoiceAsPaidDialogState _markAsPaidCommand = new() 
    { 
        InvoiceId = DefaultIdType.Empty, 
        PaidDate = DateTime.Today,
        PaymentMethod = string.Empty,
        AmountPaid = 0
    };

    /// <summary>
    /// State for applying a payment to an invoice dialog.
    /// </summary>
    private ApplyInvoicePaymentDialogState _applyPaymentCommand = new() 
    { 
        InvoiceId = DefaultIdType.Empty, 
        PaymentDate = DateTime.Today,
        Amount = 0,
        PaymentMethod = string.Empty,
        Reference = string.Empty
    };

    /// <summary>
    /// State for cancelling an invoice dialog.
    /// </summary>
    private CancelInvoiceDialogState _cancelCommand = new() 
    { 
        InvoiceId = DefaultIdType.Empty, 
        Reason = string.Empty 
    };

    /// <summary>
    /// Dialog visibility flag for void invoice dialog.
    /// </summary>
    private bool _voidDialogVisible;

    /// <summary>
    /// State for voiding an invoice dialog.
    /// </summary>
    private VoidInvoiceDialogState _voidCommand = new() 
    { 
        InvoiceId = DefaultIdType.Empty, 
        Reason = string.Empty 
    };

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
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load initial preference from localStorage
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<InvoiceResponse, DefaultIdType, InvoiceViewModel>(
            entityName: "Invoice",
            entityNamePlural: "Invoices",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<InvoiceResponse>(response => response.InvoiceNumber, "Invoice Number", "InvoiceNumber"),
                new EntityField<InvoiceResponse>(response => response.InvoiceDate, "Invoice Date", "InvoiceDate", typeof(DateOnly)),
                new EntityField<InvoiceResponse>(response => response.DueDate, "Due Date", "DueDate", typeof(DateOnly)),
                new EntityField<InvoiceResponse>(response => response.BillingPeriod, "Billing Period", "BillingPeriod"),
                new EntityField<InvoiceResponse>(response => response.TotalAmount, "Total Amount", "TotalAmount", typeof(decimal)),
                new EntityField<InvoiceResponse>(response => response.PaidAmount, "Paid Amount", "PaidAmount", typeof(decimal)),
                new EntityField<InvoiceResponse>(response => response.Status, "Status", "Status"),
                new EntityField<InvoiceResponse>(response => response.KWhUsed, "kWh Used", "KWhUsed", typeof(decimal)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchInvoicesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchInvoicesEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<InvoiceResponse>>();
            },
            createFunc: async invoice =>
            {
                var createCommand = new CreateInvoiceCommand
                {
                    InvoiceNumber = invoice.InvoiceNumber,
                    MemberId = invoice.MemberId!.Value,
                    InvoiceDate = invoice.InvoiceDate!.Value,
                    DueDate = invoice.DueDate!.Value,
                    UsageCharge = invoice.UsageCharge,
                    BasicServiceCharge = invoice.BasicServiceCharge,
                    TaxAmount = invoice.TaxAmount,
                    OtherCharges = invoice.OtherCharges,
                    KWhUsed = invoice.KWhUsed,
                    BillingPeriod = invoice.BillingPeriod,
                    LateFee = invoice.LateFee,
                    ReconnectionFee = invoice.ReconnectionFee,
                    DepositAmount = invoice.DepositAmount,
                    RateSchedule = invoice.RateSchedule,
                    DemandCharge = invoice.DemandCharge,
                    Description = invoice.Description,
                    Notes = invoice.Notes,
                    ConsumptionId = invoice.ConsumptionId
                };
                await Client.CreateInvoiceEndpointAsync("1", createCommand);
                Snackbar.Add("Invoice created successfully", Severity.Success);
            },
            updateFunc: async (id, invoice) =>
            {
                if (invoice.Status == "Paid")
                {
                    Snackbar.Add("Cannot update a paid invoice.", Severity.Error);
                    return;
                }

                if (invoice.Status == "Cancelled")
                {
                    Snackbar.Add("Cannot update a cancelled invoice.", Severity.Error);
                    return;
                }

                var updateCommand = new UpdateInvoiceCommand
                {
                    InvoiceId = id,
                    DueDate = invoice.DueDate,
                    UsageCharge = invoice.UsageCharge,
                    BasicServiceCharge = invoice.BasicServiceCharge,
                    TaxAmount = invoice.TaxAmount,
                    OtherCharges = invoice.OtherCharges,
                    LateFee = invoice.LateFee,
                    ReconnectionFee = invoice.ReconnectionFee,
                    DepositAmount = invoice.DepositAmount,
                    DemandCharge = invoice.DemandCharge,
                    RateSchedule = invoice.RateSchedule,
                    Description = invoice.Description,
                    Notes = invoice.Notes
                };
                await Client.UpdateInvoiceEndpointAsync("1", id, updateCommand);
                Snackbar.Add("Invoice updated successfully", Severity.Success);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteInvoiceEndpointAsync("1", id);
                Snackbar.Add("Invoice deleted successfully", Severity.Success);
            },
            getDefaultsFunc: () => Task.FromResult(new InvoiceViewModel
            {
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(30),
                Status = "Draft",
                BillingPeriod = DateTime.Today.ToString("yyyy-MM"),
                LineItems = []
            }),
            hasExtraActionsFunc: () => true);

        await Task.CompletedTask;
    }

    // Send Invoice
    private async Task OnSendInvoice(DefaultIdType invoiceId)
    {
        try
        {
            await Client.SendInvoiceEndpointAsync("1", invoiceId);
            Snackbar.Add("Invoice sent successfully", Severity.Success);
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error sending invoice: {ex.Message}", Severity.Error);
        }
    }

    // Mark as Paid Dialog
    private void OnMarkAsPaid(DefaultIdType invoiceId)
    {
        _markAsPaidCommand = new MarkInvoiceAsPaidDialogState
        { 
            InvoiceId = invoiceId, 
            PaidDate = DateTime.Today,
            PaymentMethod = string.Empty,
            AmountPaid = 0
        };
        _markAsPaidDialogVisible = true;
    }

    private async Task SubmitMarkAsPaid()
    {
        try
        {
            if (!_markAsPaidCommand.PaidDate.HasValue)
            {
                Snackbar.Add("Please select a payment date", Severity.Error);
                return;
            }

            var command = new MarkInvoiceAsPaidCommand
            {
                InvoiceId = _markAsPaidCommand.InvoiceId,
                PaidDate = _markAsPaidCommand.PaidDate.Value,
                PaymentMethod = _markAsPaidCommand.PaymentMethod
            };
            await Client.MarkInvoiceAsPaidEndpointAsync("1", _markAsPaidCommand.InvoiceId, command);
            Snackbar.Add("Invoice marked as paid", Severity.Success);
            _markAsPaidDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error marking invoice as paid: {ex.Message}", Severity.Error);
        }
    }

    // Apply Payment Dialog
    private void OnApplyPayment(DefaultIdType invoiceId)
    {
        _applyPaymentCommand = new ApplyInvoicePaymentDialogState 
        { 
            InvoiceId = invoiceId, 
            PaymentDate = DateTime.Today,
            Amount = 0,
            PaymentMethod = string.Empty,
            Reference = string.Empty
        };
        _applyPaymentDialogVisible = true;
    }

    private async Task SubmitApplyPayment()
    {
        try
        {
            if (!_applyPaymentCommand.PaymentDate.HasValue)
            {
                Snackbar.Add("Please select a payment date", Severity.Error);
                return;
            }
            var command = new ApplyInvoicePaymentCommand
            {
                InvoiceId = _applyPaymentCommand.InvoiceId,
                Amount = _applyPaymentCommand.Amount,
                PaymentDate = _applyPaymentCommand.PaymentDate.Value,
                PaymentMethod = _applyPaymentCommand.PaymentMethod
            };
            await Client.ApplyInvoicePaymentEndpointAsync("1", _applyPaymentCommand.InvoiceId, command);
            Snackbar.Add("Payment applied successfully", Severity.Success);
            _applyPaymentDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error applying payment: {ex.Message}", Severity.Error);
        }
    }

    // Cancel Invoice Dialog
    private void OnCancelInvoice(DefaultIdType invoiceId)
    {
        _cancelCommand = new CancelInvoiceDialogState { InvoiceId = invoiceId, Reason = string.Empty };
        _cancelDialogVisible = true;
    }

    private async Task SubmitCancelInvoice()
    {
        try
        {

            var command = new CancelInvoiceCommand
            {
                InvoiceId = _cancelCommand.InvoiceId,
                Reason = _cancelCommand.Reason
            };
            await Client.CancelInvoiceEndpointAsync("1", _cancelCommand.InvoiceId, command);
            Snackbar.Add("Invoice cancelled", Severity.Success);
            _cancelDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error cancelling invoice: {ex.Message}", Severity.Error);
        }
    }

    // Void Invoice Dialog
    private void OnVoidInvoice(DefaultIdType invoiceId)
    {
        _voidCommand = new VoidInvoiceDialogState { InvoiceId = invoiceId, Reason = string.Empty };
        _voidDialogVisible = true;
    }

    private async Task SubmitVoidInvoice()
    {
        try
        {
            var command = new VoidInvoiceCommand
            {
                InvoiceId = _voidCommand.InvoiceId,
                Reason = _voidCommand.Reason
            };
            await Client.VoidInvoiceEndpointAsync("1", _voidCommand.InvoiceId, command);
            Snackbar.Add("Invoice voided successfully", Severity.Success);
            _voidDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error voiding invoice: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Prints the invoice (placeholder for future implementation).
    /// </summary>
    private void OnPrintInvoice(DefaultIdType invoiceId)
    {
        Snackbar.Add($"Print functionality not yet implemented for invoice {invoiceId}", Severity.Info);
    }

    /// <summary>
    /// Emails the invoice (placeholder for future implementation).
    /// </summary>
    private void OnEmailInvoice(DefaultIdType invoiceId)
    {
        Snackbar.Add($"Email functionality not yet implemented for invoice {invoiceId}", Severity.Info);
    }

    // Action Navigation Menu Methods

    /// <summary>
    /// Shows invoice reports (placeholder).
    /// </summary>
    private void ShowInvoiceReports()
    {
        Snackbar.Add("Invoice reports feature coming soon", Severity.Info);
    }

    /// <summary>
    /// Shows billing periods management (placeholder).
    /// </summary>
    private void ShowBillingPeriods()
    {
        Snackbar.Add("Billing periods management feature coming soon", Severity.Info);
    }

    /// <summary>
    /// Filters to show only draft invoices.
    /// </summary>
    private async Task ShowDraftInvoices()
    {
        Status = "Draft";
        await _table.ReloadDataAsync();
        Snackbar.Add("Showing draft invoices", Severity.Info);
    }

    /// <summary>
    /// Filters to show only sent invoices.
    /// </summary>
    private async Task ShowSentInvoices()
    {
        Status = "Sent";
        await _table.ReloadDataAsync();
        Snackbar.Add("Showing sent invoices", Severity.Info);
    }

    /// <summary>
    /// Filters to show only unpaid invoices.
    /// </summary>
    private async Task ShowUnpaidInvoices()
    {
        HasOutstandingBalance = true;
        await _table.ReloadDataAsync();
        Snackbar.Add("Showing unpaid invoices", Severity.Info);
    }

    /// <summary>
    /// Filters to show only overdue invoices.
    /// </summary>
    private async Task ShowOverdueInvoices()
    {
        Status = "Overdue";
        await _table.ReloadDataAsync();
        Snackbar.Add("Showing overdue invoices", Severity.Info);
    }

    /// <summary>
    /// Exports invoices to Excel (placeholder).
    /// </summary>
    private void ExportInvoices()
    {
        Snackbar.Add("Export feature coming soon", Severity.Info);
    }

    /// <summary>
    /// Shows invoice settings (placeholder).
    /// </summary>
    private void ShowSettings()
    {
        Snackbar.Add("Settings feature coming soon", Severity.Info);
    }

    // View Invoice Details with Line Items
    private async Task ViewInvoiceDetails(DefaultIdType invoiceId)
    {
        var parameters = new DialogParameters<InvoiceDetailsDialog>
        {
            { x => x.InvoiceId, invoiceId }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<InvoiceDetailsDialog>("Invoice Details", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Show invoices help dialog.
    /// </summary>
    private async Task ShowInvoicesHelp()
    {
        await DialogService.ShowAsync<InvoicesHelpDialog>("Invoices Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

// Supporting types for dialog state
public sealed class MarkInvoiceAsPaidDialogState
{
    public DefaultIdType InvoiceId { get; set; }
    public DateTime? PaidDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal AmountPaid { get; set; }
}

public sealed class ApplyInvoicePaymentDialogState
{
    public DefaultIdType InvoiceId { get; set; }
    public DateTime? PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
}

public sealed class CancelInvoiceDialogState
{
    public DefaultIdType InvoiceId { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public sealed class VoidInvoiceDialogState
{
    public DefaultIdType InvoiceId { get; set; }
    public string Reason { get; set; } = string.Empty;
}

