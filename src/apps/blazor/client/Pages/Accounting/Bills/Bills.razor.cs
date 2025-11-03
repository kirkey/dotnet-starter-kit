namespace FSH.Starter.Blazor.Client.Pages.Accounting.Bills;

public partial class Bills
{
    protected EntityServerTableContext<BillSearchResponse, DefaultIdType, BillViewModel> Context { get; set; } = default!;

    private EntityTable<BillSearchResponse, DefaultIdType, BillViewModel> _table = default!;

    // Search filters
    private string? BillNumber { get; set; }
    private string? VendorName { get; set; }
    private string? Status { get; set; }
    private string? ApprovalStatus { get; set; }
    private DateTime? BillDateFrom { get; set; }
    private DateTime? BillDateTo { get; set; }
    private DateTime? DueDateFrom { get; set; }
    private DateTime? DueDateTo { get; set; }
    private bool IsPosted { get; set; }
    private bool IsPaid { get; set; }

    // Dialog visibility flags
    private bool _approveDialogVisible;
    private bool _rejectDialogVisible;
    private bool _markAsPaidDialogVisible;
    private bool _voidDialogVisible;

    // Dialog options
    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

    // Command objects for dialogs
    private ApproveBillCommand _approveCommand = new(DefaultIdType.Empty, string.Empty);
    private RejectBillCommand _rejectCommand = new(DefaultIdType.Empty, string.Empty, string.Empty);
    private MarkBillAsPaidCommand _markAsPaidCommand = new(DefaultIdType.Empty, DateTime.UtcNow);
    private VoidBillCommand _voidCommand = new(DefaultIdType.Empty, string.Empty);

    // Get status color for badges
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
                var searchQuery = new BillSearchQuery
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
                var result = await Client.BillSearchEndpointAsync("1", searchQuery);
                return result.Adapt<PaginationResponse<BillSearchResponse>>();
            },
            createFunc: async bill =>
            {
                var createCommand = new BillCreateCommand(
                    bill.BillNumber,
                    bill.VendorId!.Value,
                    bill.BillDate!.Value,
                    bill.DueDate!.Value,
                    bill.Description,
                    bill.PeriodId,
                    bill.PaymentTerms,
                    bill.PurchaseOrderNumber,
                    bill.Notes,
                    bill.LineItems?.Select(li => new BillLineItemDto(
                        li.LineNumber,
                        li.Description,
                        li.Quantity,
                        li.UnitPrice,
                        li.Amount,
                        li.ChartOfAccountId!.Value,
                        li.TaxCodeId,
                        li.TaxAmount,
                        li.ProjectId,
                        li.CostCenterId,
                        li.Notes
                    )).ToList()
                );
                await Client.BillCreateEndpointAsync("1", createCommand);
            },
            updateFunc: async (id, bill) =>
            {
                var updateCommand = new BillUpdateCommand(
                    id,
                    bill.BillNumber,
                    bill.BillDate,
                    bill.DueDate,
                    bill.Description,
                    bill.PeriodId,
                    bill.PaymentTerms,
                    bill.PurchaseOrderNumber,
                    bill.Notes
                );
                await Client.BillUpdateEndpointAsync("1", id, updateCommand);
            },
            deleteFunc: async id =>
            {
                await Client.BillDeleteEndpointAsync("1", id);
            },
            getDetailsFunc: async id =>
            {
                var response = await Client.BillGetByIdEndpointAsync("1", id);
                return response.Adapt<BillViewModel>();
            },
            hasExtraActionsFunc: () => true);

        return Task.CompletedTask;
    }

    // Approve Bill Dialog
    private void OnApproveBill(DefaultIdType billId)
    {
        _approveCommand = new ApproveBillCommand(billId, string.Empty);
        _approveDialogVisible = true;
    }

    private async Task SubmitApproveBill()
    {
        try
        {
            await Client.BillApproveEndpointAsync("1", _approveCommand.BillId, _approveCommand);
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
        _rejectCommand = new RejectBillCommand(billId, string.Empty, string.Empty);
        _rejectDialogVisible = true;
    }

    private async Task SubmitRejectBill()
    {
        try
        {
            await Client.BillRejectEndpointAsync("1", _rejectCommand.BillId, _rejectCommand);
            Snackbar.Add("Bill rejected", Severity.Success);
            _rejectDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error rejecting bill: {ex.Message}", Severity.Error);
        }
    }

    // Post Bill
    private async Task OnPostBill(DefaultIdType billId)
    {
        try
        {
            await Client.BillPostEndpointAsync("1", billId);
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
        _markAsPaidCommand = new MarkBillAsPaidCommand(billId, DateTime.Today);
        _markAsPaidDialogVisible = true;
    }

    private async Task SubmitMarkAsPaid()
    {
        try
        {
            await Client.BillMarkAsPaidEndpointAsync("1", _markAsPaidCommand.BillId, _markAsPaidCommand);
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
        _voidCommand = new VoidBillCommand(billId, string.Empty);
        _voidDialogVisible = true;
    }

    private async Task SubmitVoidBill()
    {
        try
        {
            await Client.BillVoidEndpointAsync("1", _voidCommand.BillId, _voidCommand);
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
    private void OnPrintBill(DefaultIdType billId)
    {
        Snackbar.Add("Print functionality not yet implemented", Severity.Info);
    }
}

// Supporting types for API calls
public sealed record BillSearchQuery : PaginationFilter
{
    public string? BillNumber { get; init; }
    public DefaultIdType? VendorId { get; init; }
    public string? Status { get; init; }
    public string? ApprovalStatus { get; init; }
    public DateTime? BillDateFrom { get; init; }
    public DateTime? BillDateTo { get; init; }
    public DateTime? DueDateFrom { get; init; }
    public DateTime? DueDateTo { get; init; }
    public bool? IsPosted { get; init; }
    public bool? IsPaid { get; init; }
    public DefaultIdType? PeriodId { get; init; }
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

public sealed record BillCreateCommand(
    string BillNumber,
    DefaultIdType VendorId,
    DateTime BillDate,
    DateTime DueDate,
    string? Description = null,
    DefaultIdType? PeriodId = null,
    string? PaymentTerms = null,
    string? PurchaseOrderNumber = null,
    string? Notes = null,
    List<BillLineItemDto>? LineItems = null
);

public sealed record BillLineItemDto(
    int LineNumber,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal Amount,
    DefaultIdType ChartOfAccountId,
    DefaultIdType? TaxCodeId = null,
    decimal TaxAmount = 0,
    DefaultIdType? ProjectId = null,
    DefaultIdType? CostCenterId = null,
    string? Notes = null
);

public sealed record BillUpdateCommand(
    DefaultIdType BillId,
    string? BillNumber = null,
    DateTime? BillDate = null,
    DateTime? DueDate = null,
    string? Description = null,
    DefaultIdType? PeriodId = null,
    string? PaymentTerms = null,
    string? PurchaseOrderNumber = null,
    string? Notes = null
);

public sealed record ApproveBillCommand(DefaultIdType BillId, string ApprovedBy);
public sealed record RejectBillCommand(DefaultIdType BillId, string RejectedBy, string Reason);
public sealed record MarkBillAsPaidCommand(DefaultIdType BillId, DateTime PaidDate);
public sealed record VoidBillCommand(DefaultIdType BillId, string Reason);

