namespace FSH.Starter.Blazor.Client.Pages.Accounting.CreditMemos;

public partial class CreditMemos
{
    protected EntityServerTableContext<CreditMemoSearchResponse, DefaultIdType, CreditMemoViewModel> Context { get; set; } = default!;

    private EntityTable<CreditMemoSearchResponse, DefaultIdType, CreditMemoViewModel> _table = default!;

    // Dialog visibility flags
    private bool _approveDialogVisible;
    private bool _applyDialogVisible;
    private bool _refundDialogVisible;
    private bool _voidDialogVisible;

    // Dialog options
    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

    // Command objects for dialogs
    private CreditMemoApproveCommand _approveCommand = new() { CreditMemoId = Guid.Empty, ApprovedBy = string.Empty, ApprovedDate = DateTime.UtcNow };
    private CreditMemoApplyCommand _applyCommand = new() { CreditMemoId = Guid.Empty, DocumentId = Guid.Empty, AmountToApply = 0, AppliedDate = DateTime.UtcNow };
    private CreditMemoRefundCommand _refundCommand = new() { CreditMemoId = Guid.Empty, RefundAmount = 0, RefundDate = DateTime.UtcNow, RefundMethod = string.Empty, RefundReference = string.Empty };
    private CreditMemoVoidCommand _voidCommand = new() { CreditMemoId = Guid.Empty, VoidReason = string.Empty };

    // Get status color for badges
    private static Color GetStatusColor(string? status) => status switch
    {
        "Draft" => Color.Default,
        "Approved" => Color.Info,
        "Applied" => Color.Success,
        "Refunded" => Color.Primary,
        "Voided" => Color.Error,
        _ => Color.Default
    };

    private static Color GetApprovalStatusColor(string? approvalStatus) => approvalStatus switch
    {
        "Pending" => Color.Warning,
        "Approved" => Color.Success,
        "Rejected" => Color.Error,
        _ => Color.Default
    };

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<CreditMemoSearchResponse, DefaultIdType, CreditMemoViewModel>(
            entityName: "Credit Memo",
            entityNamePlural: "Credit Memos",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<CreditMemoSearchResponse>(response => response.MemoNumber, "Memo Number", "MemoNumber"),
                new EntityField<CreditMemoSearchResponse>(response => response.MemoDate, "Date", "MemoDate", typeof(DateTime)),
                new EntityField<CreditMemoSearchResponse>(response => response.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<CreditMemoSearchResponse>(response => response.AppliedAmount, "Applied", "AppliedAmount", typeof(decimal)),
                new EntityField<CreditMemoSearchResponse>(response => response.RefundedAmount, "Refunded", "RefundedAmount", typeof(decimal)),
                new EntityField<CreditMemoSearchResponse>(response => response.UnappliedAmount, "Unapplied", "UnappliedAmount", typeof(decimal)),
                new EntityField<CreditMemoSearchResponse>(response => response.ReferenceType, "Type", "ReferenceType"),
                new EntityField<CreditMemoSearchResponse>(response => response.Status, "Status", "Status"),
                new EntityField<CreditMemoSearchResponse>(response => response.ApprovalStatus, "Approval", "ApprovalStatus"),
                new EntityField<CreditMemoSearchResponse>(response => response.Reason, "Reason", "Reason"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<CreditMemoSearchQuery>();
                var result = await Client.CreditMemoSearchEndpointAsync(paginationFilter);
                return result.Adapt<PaginationResponse<CreditMemoSearchResponse>>();
            },
            createFunc: async creditMemo =>
            {
                await Client.CreditMemoCreateEndpointAsync(creditMemo.Adapt<CreditMemoCreateCommand>());
            },
            updateFunc: async (id, creditMemo) =>
            {
                var updateCommand = new CreditMemoUpdateCommand
                {
                    Id = id,
                    MemoDate = creditMemo.MemoDate,
                    Amount = creditMemo.Amount,
                    Reason = creditMemo.Reason,
                    Description = creditMemo.Description,
                    Notes = creditMemo.Notes
                };
                await Client.CreditMemoUpdateEndpointAsync(id, updateCommand);
            },
            deleteFunc: async id =>
            {
                // Only allow deletion of Draft memos
                var memoDetails = await Client.CreditMemoGetEndpointAsync(id);
                if (memoDetails.Status != "Draft")
                {
                    Snackbar.Add("Only draft credit memos can be deleted.", Severity.Warning);
                    return;
                }
                await Client.CreditMemoDeleteEndpointAsync(id);
            },
            getDetailsFunc: async id =>
            {
                var response = await Client.CreditMemoGetEndpointAsync(id);
                return response.Adapt<CreditMemoViewModel>();
            },
            hasExtraActionsFunc: () => true);

        return Task.CompletedTask;
    }

    // Approve Credit Memo Dialog
    private void OnApproveMemo(DefaultIdType creditMemoId)
    {
        _approveCommand = new CreditMemoApproveCommand 
        { 
            CreditMemoId = creditMemoId, 
            ApprovedBy = string.Empty, 
            ApprovedDate = DateTime.UtcNow 
        };
        _approveDialogVisible = true;
    }

    private async Task SubmitApproveMemo()
    {
        if (string.IsNullOrWhiteSpace(_approveCommand.ApprovedBy))
        {
            Snackbar.Add("Please enter who is approving this memo.", Severity.Error);
            return;
        }

        try
        {
            await Client.CreditMemoApproveEndpointAsync(_approveCommand.CreditMemoId, _approveCommand);
            Snackbar.Add("Credit memo approved successfully.", Severity.Success);
            _approveDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error approving credit memo: {ex.Message}", Severity.Error);
        }
    }

    // Apply Credit Memo Dialog
    private void OnApplyMemo(DefaultIdType creditMemoId)
    {
        _applyCommand = new CreditMemoApplyCommand 
        { 
            CreditMemoId = creditMemoId, 
            DocumentId = Guid.Empty, 
            AmountToApply = 0, 
            AppliedDate = DateTime.UtcNow 
        };
        _applyDialogVisible = true;
    }

    private async Task SubmitApplyMemo()
    {
        if (_applyCommand.DocumentId == Guid.Empty)
        {
            Snackbar.Add("Please enter a document ID to apply the memo to.", Severity.Error);
            return;
        }

        if (_applyCommand.AmountToApply <= 0)
        {
            Snackbar.Add("Amount to apply must be greater than zero.", Severity.Error);
            return;
        }

        try
        {
            await Client.CreditMemoApplyEndpointAsync(_applyCommand.CreditMemoId, _applyCommand);
            Snackbar.Add("Credit memo applied successfully.", Severity.Success);
            _applyDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error applying credit memo: {ex.Message}", Severity.Error);
        }
    }

    // Refund Credit Memo Dialog
    private void OnRefundMemo(DefaultIdType creditMemoId)
    {
        _refundCommand = new CreditMemoRefundCommand 
        { 
            CreditMemoId = creditMemoId, 
            RefundAmount = 0, 
            RefundDate = DateTime.UtcNow, 
            RefundMethod = string.Empty, 
            RefundReference = string.Empty 
        };
        _refundDialogVisible = true;
    }

    private async Task SubmitRefundMemo()
    {
        if (_refundCommand.RefundAmount <= 0)
        {
            Snackbar.Add("Refund amount must be greater than zero.", Severity.Error);
            return;
        }

        if (string.IsNullOrWhiteSpace(_refundCommand.RefundMethod))
        {
            Snackbar.Add("Please specify the refund method.", Severity.Error);
            return;
        }

        try
        {
            await Client.CreditMemoRefundEndpointAsync(_refundCommand.CreditMemoId, _refundCommand);
            Snackbar.Add("Credit memo refunded successfully.", Severity.Success);
            _refundDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error refunding credit memo: {ex.Message}", Severity.Error);
        }
    }

    // Void Credit Memo Dialog
    private void OnVoidMemo(DefaultIdType creditMemoId)
    {
        _voidCommand = new CreditMemoVoidCommand { CreditMemoId = creditMemoId, VoidReason = string.Empty };
        _voidDialogVisible = true;
    }

    private async Task SubmitVoidMemo()
    {
        if (string.IsNullOrWhiteSpace(_voidCommand.VoidReason))
        {
            Snackbar.Add("Please provide a reason for voiding this memo.", Severity.Error);
            return;
        }

        try
        {
            await Client.CreditMemoVoidEndpointAsync(_voidCommand.CreditMemoId, _voidCommand);
            Snackbar.Add("Credit memo voided successfully.", Severity.Success);
            _voidDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error voiding credit memo: {ex.Message}", Severity.Error);
        }
    }

    // View Applications
    private void OnViewApplications(DefaultIdType creditMemoId)
    {
        // Navigate to applications view or show dialog with application history
        Snackbar.Add("View applications feature coming soon.", Severity.Info);
    }
}

// Command objects for API calls
public record CreditMemoApproveCommand
{
    public DefaultIdType CreditMemoId { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTime ApprovedDate { get; set; }
}

public record CreditMemoApplyCommand
{
    public DefaultIdType CreditMemoId { get; set; }
    public DefaultIdType DocumentId { get; set; }
    public decimal AmountToApply { get; set; }
    public DateTime AppliedDate { get; set; }
}

public record CreditMemoRefundCommand
{
    public DefaultIdType CreditMemoId { get; set; }
    public decimal RefundAmount { get; set; }
    public DateTime RefundDate { get; set; }
    public string RefundMethod { get; set; } = string.Empty;
    public string RefundReference { get; set; } = string.Empty;
}

public record CreditMemoVoidCommand
{
    public DefaultIdType CreditMemoId { get; set; }
    public string VoidReason { get; set; } = string.Empty;
}

// Search request extension (if not already defined in API client)
public class CreditMemoSearchRequest
{
    public string? MemoNumber { get; set; }
    public string? ReferenceType { get; set; }
    public string? Status { get; set; }
    public string? ApprovalStatus { get; set; }
    public decimal? AmountFrom { get; set; }
    public decimal? AmountTo { get; set; }
    public DateTime? MemoDateFrom { get; set; }
    public DateTime? MemoDateTo { get; set; }
    public bool HasUnappliedAmount { get; set; }
}
