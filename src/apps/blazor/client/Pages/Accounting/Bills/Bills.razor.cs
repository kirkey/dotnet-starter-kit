namespace FSH.Starter.Blazor.Client.Pages.Accounting.Bills;

/// <summary>
/// Bills page for managing vendor bills and accounts payable.
/// </summary>
public partial class Bills
{
    /// <summary>
    /// The entity table context for managing bills with server-side operations.
    /// </summary>
    protected EntityServerTableContext<BillSearchResponse, DefaultIdType, BillViewModel> Context { get; set; } = default!;

    /// <summary>
    /// Reference to the EntityTable component for bills.
    /// </summary>
    private EntityTable<BillSearchResponse, DefaultIdType, BillViewModel> _table = default!;

    /// <summary>
    /// Search filter for bill number.
    /// </summary>
    private string? BillNumber { get; set; }

    /// <summary>
    /// Search filter for vendor name.
    /// </summary>
    private string? VendorName { get; set; }

    /// <summary>
    /// Search filter for bill status.
    /// </summary>
    private string? Status { get; set; }

    /// <summary>
    /// Search filter for approval status.
    /// </summary>
    private string? ApprovalStatus { get; set; }

    /// <summary>
    /// Search filter for bill date range start.
    /// </summary>
    private DateTime? BillDateFrom { get; set; }

    /// <summary>
    /// Search filter for bill date range end.
    /// </summary>
    private DateTime? BillDateTo { get; set; }

    /// <summary>
    /// Search filter for due date range start.
    /// </summary>
    private DateTime? DueDateFrom { get; set; }

    /// <summary>
    /// Search filter for due date range end.
    /// </summary>
    private DateTime? DueDateTo { get; set; }

    /// <summary>
    /// Search filter for posted status.
    /// </summary>
    private bool IsPosted { get; set; }

    /// <summary>
    /// Search filter for paid status.
    /// </summary>
    private bool IsPaid { get; set; }

    /// <summary>
    /// Dialog visibility flag for approve bill dialog.
    /// </summary>
    private bool _approveDialogVisible;

    /// <summary>
    /// Dialog visibility flag for reject bill dialog.
    /// </summary>
    private bool _rejectDialogVisible;

    /// <summary>
    /// Dialog visibility flag for mark as paid dialog.
    /// </summary>
    private bool _markAsPaidDialogVisible;

    /// <summary>
    /// Dialog visibility flag for void bill dialog.
    /// </summary>
    private bool _voidDialogVisible;

    /// <summary>
    /// Dialog options for modal dialogs.
    /// </summary>
    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

    /// <summary>
    /// Command for approving a bill.
    /// </summary>
    private ApproveBillCommand _approveCommand = new() { BillId = DefaultIdType.Empty, ApprovedBy = string.Empty };

    /// <summary>
    /// Command for rejecting a bill.
    /// </summary>
    private RejectBillCommand _rejectCommand = new() { BillId = DefaultIdType.Empty, RejectedBy = string.Empty, Reason = string.Empty };

    /// <summary>
    /// Command for marking a bill as paid.
    /// </summary>
    private MarkBillAsPaidCommand _markAsPaidCommand = new() { BillId = DefaultIdType.Empty, PaidDate = DateTime.UtcNow };

    /// <summary>
    /// Command for voiding a bill.
    /// </summary>
    private VoidBillCommand _voidCommand = new() { BillId = DefaultIdType.Empty, Reason = string.Empty };

    /// <summary>
    /// Gets the status color based on bill status.
    /// </summary>
    private static Color GetStatusColor(string? status) => status switch
    {
        "Draft" => Color.Default,
        "Submitted" => Color.Info,
        "Approved" => Color.Success,
        "Rejected" => Color.Error,
        "Posted" => Color.Primary,
        "Paid" => Color.Success,
        "Void" => Color.Warning,
        _ => Color.Default
    };

    /// <summary>
    /// Gets the severity for total amount indicator.
    /// </summary>
    private static Severity GetTotalSeverity(BillViewModel model) =>
        model.LineItems.Count > 0 ? Severity.Success : Severity.Warning;

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<BillSearchResponse, DefaultIdType, BillViewModel>(
            entityName: "Bill",
            entityNamePlural: "Bills",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<BillSearchResponse>(response => response.BillNumber, "Bill Number", "BillNumber"),
                new EntityField<BillSearchResponse>(response => response.VendorName, "Vendor", "VendorName"),
                new EntityField<BillSearchResponse>(response => response.BillDate, "Bill Date", "BillDate", typeof(DateTime)),
                new EntityField<BillSearchResponse>(response => response.DueDate, "Due Date", "DueDate", typeof(DateTime)),
                new EntityField<BillSearchResponse>(response => response.TotalAmount, "Amount", "TotalAmount", typeof(decimal)),
                new EntityField<BillSearchResponse>(response => response.Status, "Status", "Status"),
                new EntityField<BillSearchResponse>(response => response.ApprovalStatus, "Approval", "ApprovalStatus"),
                new EntityField<BillSearchResponse>(response => response.IsPosted, "Posted", "IsPosted", typeof(bool)),
                new EntityField<BillSearchResponse>(response => response.IsPaid, "Paid", "IsPaid", typeof(bool)),
                new EntityField<BillSearchResponse>(response => response.PaymentTerms, "Terms", "PaymentTerms"),
                new EntityField<BillSearchResponse>(response => response.LineItemCount, "Lines", "LineItemCount", typeof(int)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var searchQuery = new SearchBillsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    OrderBy = filter.OrderBy,
                    Keyword = filter.Keyword,
                    BillNumber = BillNumber,
                    Status = Status,
                    ApprovalStatus = ApprovalStatus,
                    BillDateFrom = BillDateFrom,
                    BillDateTo = BillDateTo,
                    DueDateFrom = DueDateFrom,
                    DueDateTo = DueDateTo,
                    IsPosted = IsPosted ? true : null,
                    IsPaid = IsPaid ? true : null
                };
                var result = await Client.SearchBillsEndpointAsync("1", searchQuery);
                return result.Adapt<PaginationResponse<BillSearchResponse>>();
            },
            createFunc: async bill =>
            {
                if (bill.LineItems.Count == 0)
                {
                    Snackbar.Add("At least one line item is required.", Severity.Error);
                    return;
                }

                var createCommand = new BillCreateCommand
                {
                    BillNumber = bill.BillNumber,
                    VendorId = bill.VendorId!.Value,
                    BillDate = bill.BillDate!.Value,
                    DueDate = bill.DueDate!.Value,
                    Description = bill.Description,
                    PeriodId = bill.PeriodId,
                    PaymentTerms = bill.PaymentTerms,
                    PurchaseOrderNumber = bill.PurchaseOrderNumber,
                    Notes = bill.Notes,
                    LineItems = bill.LineItems.Select(li => new BillLineItemDto
                    {
                        LineNumber = li.LineNumber,
                        Description = li.Description,
                        Quantity = li.Quantity,
                        UnitPrice = li.UnitPrice,
                        Amount = li.Amount,
                        ChartOfAccountId = li.ChartOfAccountId!.Value,
                        TaxCodeId = li.TaxCodeId,
                        TaxAmount = li.TaxAmount,
                        ProjectId = li.ProjectId,
                        CostCenterId = li.CostCenterId,
                        Notes = li.Notes
                    }).ToList()
                };
                await Client.BillCreateEndpointAsync("1", createCommand);
                Snackbar.Add("Bill created successfully", Severity.Success);
            },
            updateFunc: async (id, bill) =>
            {
                if (bill.IsPosted)
                {
                    Snackbar.Add("Cannot update a posted bill.", Severity.Error);
                    return;
                }

                if (bill.IsPaid)
                {
                    Snackbar.Add("Cannot update a paid bill.", Severity.Error);
                    return;
                }

                var updateCommand = new BillUpdateCommand
                {
                    BillId = id,
                    BillNumber = bill.BillNumber,
                    BillDate = bill.BillDate,
                    DueDate = bill.DueDate,
                    Description = bill.Description,
                    PeriodId = bill.PeriodId,
                    PaymentTerms = bill.PaymentTerms,
                    PurchaseOrderNumber = bill.PurchaseOrderNumber,
                    Notes = bill.Notes
                };
                await Client.BillUpdateEndpointAsync("1", id, updateCommand);
                Snackbar.Add("Bill updated successfully", Severity.Success);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteBillEndpointAsync("1", id);
                Snackbar.Add("Bill deleted successfully", Severity.Success);
            },
            getDetailsFunc: async id =>
            {
                var response = await Client.GetBillEndpointAsync("1", id);
                var lineItems = await Client.GetBillLineItemsEndpointAsync("1", id);
                
                var viewModel = response.Adapt<BillViewModel>();
                viewModel.LineItems = lineItems.Select(li => new BillLineItemViewModel
                {
                    Id = li.Id,
                    BillId = id,
                    LineNumber = li.LineNumber,
                    Description = li.Description ?? string.Empty,
                    Quantity = li.Quantity,
                    UnitPrice = li.UnitPrice,
                    Amount = li.Amount,
                    ChartOfAccountId = li.ChartOfAccountId,
                    ChartOfAccountCode = li.ChartOfAccountCode,
                    ChartOfAccountName = li.ChartOfAccountName,
                    TaxCodeId = li.TaxCodeId,
                    TaxAmount = li.TaxAmount,
                    ProjectId = li.ProjectId,
                    CostCenterId = li.CostCenterId,
                    Notes = li.Notes
                }).ToList();
                
                return viewModel;
            },
            getDefaultsFunc: () => Task.FromResult(new BillViewModel
            {
                BillDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(30),
                Status = "Draft",
                ApprovalStatus = "Pending",
                LineItems = new List<BillLineItemViewModel>()
            }),
            hasExtraActionsFunc: () => true);

        return Task.CompletedTask;
    }

    // Approve Bill Dialog
    private void OnApproveBill(DefaultIdType billId)
    {
        _approveCommand = new ApproveBillCommand { BillId = billId, ApprovedBy = string.Empty };
        _approveDialogVisible = true;
    }

    private async Task SubmitApproveBill()
    {
        try
        {
            var command = new ApproveBillRequest
            {
                ApprovedBy = _approveCommand.ApprovedBy
            };
            await Client.ApproveBillEndpointAsync("1", _approveCommand.BillId, command);
            Snackbar.Add("Bill approved successfully", Severity.Success);
            _approveDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error approving bill: {ex.Message}", Severity.Error);
        }
    }

    // Reject Bill Dialog
    private void OnRejectBill(DefaultIdType billId)
    {
        _rejectCommand = new RejectBillCommand { BillId = billId, RejectedBy = string.Empty, Reason = string.Empty };
        _rejectDialogVisible = true;
    }

    private async Task SubmitRejectBill()
    {
        try
        {
            var command = new MarkBillAsPaidRequest
            {
                PaidDate = _markAsPaidCommand.PaidDate ?? DateTime.Today
            };
            await Client.MarkBillAsPaidEndpointAsync("1", _markAsPaidCommand.BillId, command);
            Snackbar.Add("Bill marked as paid", Severity.Success);
            _markAsPaidDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error marking bill as paid: {ex.Message}", Severity.Error);
        }
    }

    // Post Bill
    private async Task OnPostBill(DefaultIdType billId)
    {
        try
        {
            await Client.PostBillEndpointAsync("1", billId);
            Snackbar.Add("Bill posted to general ledger", Severity.Success);
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error posting bill: {ex.Message}", Severity.Error);
        }
    }

    // Mark as Paid Dialog
    private void OnMarkAsPaid(DefaultIdType billId)
    {
        _markAsPaidCommand = new MarkBillAsPaidCommand { BillId = billId, PaidDate = DateTime.Today };
        _markAsPaidDialogVisible = true;
    }

    private async Task SubmitMarkAsPaid()
    {
        try
        {
            var command = new MarkBillAsPaidRequest
            {
                PaidDate = _markAsPaidCommand.PaidDate ?? DateTime.Today
            };
            await Client.MarkBillAsPaidEndpointAsync("1", _markAsPaidCommand.BillId, command);
            Snackbar.Add("Bill marked as paid", Severity.Success);
            _markAsPaidDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error marking bill as paid: {ex.Message}", Severity.Error);
        }
    }

    // Void Bill Dialog
    private void OnVoidBill(DefaultIdType billId)
    {
        _voidCommand = new VoidBillCommand { BillId = billId, Reason = string.Empty };
        _voidDialogVisible = true;
    }

    private async Task SubmitVoidBill()
    {
        try
        {
            var command = new VoidBillRequest
            {
                Reason = _voidCommand.Reason
            };
            await Client.VoidBillEndpointAsync("1", _voidCommand.BillId, command);
            Snackbar.Add("Bill voided", Severity.Success);
            _voidDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error voiding bill: {ex.Message}", Severity.Error);
        }
    }

    // Print Bill
    private void OnPrintBill(DefaultIdType _)
    {
        Snackbar.Add("Print functionality not yet implemented", Severity.Info);
    }

    // View Bill Details with Line Items
    private async Task ViewBillDetails(DefaultIdType billId)
    {
        var parameters = new DialogParameters<BillDetailsDialog>
        {
            { x => x.BillId, billId }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<BillDetailsDialog>("Bill Details", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }
}

// Supporting types for API calls
public sealed class BillSearchQuery : PaginationFilter
{
    public string? BillNumber { get; set; }
    public DefaultIdType? VendorId { get; set; }
    public string? Status { get; set; }
    public string? ApprovalStatus { get; set; }
    public DateTime? BillDateFrom { get; set; }
    public DateTime? BillDateTo { get; set; }
    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
    public bool? IsPosted { get; set; }
    public bool? IsPaid { get; set; }
    public DefaultIdType? PeriodId { get; set; }
}

public sealed record BillSearchResponse
{
    public DefaultIdType Id { get; init; }
    public string BillNumber { get; init; } = string.Empty;
    public DefaultIdType VendorId { get; init; }
    public string? VendorName { get; init; }
    public DateTime BillDate { get; init; }
    public DateTime DueDate { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsPosted { get; init; }
    public bool IsPaid { get; init; }
    public DateTime? PaidDate { get; init; }
    public string ApprovalStatus { get; init; } = string.Empty;
    public string? ApprovedBy { get; init; }
    public DefaultIdType? PeriodId { get; init; }
    public string? PaymentTerms { get; init; }
    public string? PurchaseOrderNumber { get; init; }
    public int LineItemCount { get; init; }
}

// Command classes with mutable properties for dialog binding
public sealed class ApproveBillCommand
{
    public DefaultIdType BillId { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}

public sealed class RejectBillCommand
{
    public DefaultIdType BillId { get; set; }
    public string RejectedBy { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
}

public sealed class MarkBillAsPaidCommand
{
    public DefaultIdType BillId { get; set; }
    public DateTime? PaidDate { get; set; }
}

public sealed class VoidBillCommand
{
    public DefaultIdType BillId { get; set; }
    public string Reason { get; set; } = string.Empty;
}

