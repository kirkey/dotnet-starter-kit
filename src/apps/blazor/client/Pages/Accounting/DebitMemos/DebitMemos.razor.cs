namespace FSH.Starter.Blazor.Client.Pages.Accounting.DebitMemos;

public partial class DebitMemos
{
    protected EntityServerTableContext<DebitMemoSearchResponse, DefaultIdType, DebitMemoViewModel> Context { get; set; } = default!;

    private EntityTable<DebitMemoSearchResponse, DefaultIdType, DebitMemoViewModel> _table = default!;

    // Dialog visibility flags
    private bool _approveDialogVisible;
    private bool _applyDialogVisible;
    private bool _voidDialogVisible;

    // Dialog options
    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

    // Command objects for dialogs
    private DebitMemoApproveCommand _approveCommand = new() { DebitMemoId = Guid.Empty, ApprovedBy = string.Empty, ApprovedDate = DateTime.UtcNow };
    private DebitMemoApplyCommand _applyCommand = new() { DebitMemoId = Guid.Empty, DocumentId = Guid.Empty, AmountToApply = 0, AppliedDate = DateTime.UtcNow };
    private DebitMemoVoidCommand _voidCommand = new() { DebitMemoId = Guid.Empty, VoidReason = string.Empty };

    // Get status color for badges
    private static Color GetStatusColor(string? status) => status switch
    {
        "Draft" => Color.Default,
        "Approved" => Color.Info,
        "Applied" => Color.Success,
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
        Context = new EntityServerTableContext<DebitMemoSearchResponse, DefaultIdType, DebitMemoViewModel>(
            entityName: "Debit Memo",
            entityNamePlural: "Debit Memos",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<DebitMemoSearchResponse>(response => response.MemoNumber, "Memo Number", "MemoNumber"),
                new EntityField<DebitMemoSearchResponse>(response => response.MemoDate, "Date", "MemoDate", typeof(DateTime)),
                new EntityField<DebitMemoSearchResponse>(response => response.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<DebitMemoSearchResponse>(response => response.AppliedAmount, "Applied", "AppliedAmount", typeof(decimal)),
                new EntityField<DebitMemoSearchResponse>(response => response.UnappliedAmount, "Unapplied", "UnappliedAmount", typeof(decimal)),
                new EntityField<DebitMemoSearchResponse>(response => response.ReferenceType, "Type", "ReferenceType"),
                new EntityField<DebitMemoSearchResponse>(response => response.Status, "Status", "Status"),
                new EntityField<DebitMemoSearchResponse>(response => response.ApprovalStatus, "Approval", "ApprovalStatus"),
                new EntityField<DebitMemoSearchResponse>(response => response.Reason, "Reason", "Reason"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<DebitMemoSearchQuery>();
                var result = await Client.DebitMemoSearchEndpointAsync(paginationFilter);
                return result.Adapt<PaginationResponse<DebitMemoSearchResponse>>();
            },
            createFunc: async debitMemo =>
            {
                await Client.DebitMemoCreateEndpointAsync(debitMemo.Adapt<DebitMemoCreateCommand>());
            },
            updateFunc: async (id, debitMemo) =>
            {
                var updateCommand = new DebitMemoUpdateCommand
                {
                    Id = id,
                    MemoDate = debitMemo.MemoDate,
                    Amount = debitMemo.Amount,
                    Reason = debitMemo.Reason,
                    Description = debitMemo.Description,
                    Notes = debitMemo.Notes
                };
                await Client.DebitMemoUpdateEndpointAsync(id, updateCommand);
            },
            deleteFunc: async id =>
            {
                // Only allow deletion of Draft memos
                var memoDetails = await Client.DebitMemoGetEndpointAsync(id);
                if (memoDetails.Status != "Draft")
                {
                    Snackbar.Add("Only draft debit memos can be deleted.", Severity.Warning);
                    return;
                }
                await Client.DebitMemoDeleteEndpointAsync(id);
            },
            getDetailsFunc: async id =>
            {
                var response = await Client.DebitMemoGetEndpointAsync(id);
                return response.Adapt<DebitMemoViewModel>();
            },
            hasExtraActionsFunc: () => true);

        return Task.CompletedTask;
    }

    // Approve Debit Memo Dialog
    private void OnApproveMemo(DefaultIdType debitMemoId)
    {
        _approveCommand = new DebitMemoApproveCommand 
        { 
            DebitMemoId = debitMemoId, 
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
            await Client.DebitMemoApproveEndpointAsync(_approveCommand.DebitMemoId, _approveCommand);
            Snackbar.Add("Debit memo approved successfully.", Severity.Success);
            _approveDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error approving debit memo: {ex.Message}", Severity.Error);
        }
    }

    // Apply Debit Memo Dialog
    private void OnApplyMemo(DefaultIdType debitMemoId)
    {
        _applyCommand = new DebitMemoApplyCommand 
        { 
            DebitMemoId = debitMemoId, 
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
            await Client.DebitMemoApplyEndpointAsync(_applyCommand.DebitMemoId, _applyCommand);
            Snackbar.Add("Debit memo applied successfully.", Severity.Success);
            _applyDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error applying debit memo: {ex.Message}", Severity.Error);
        }
    }

    // Void Debit Memo Dialog
    private void OnVoidMemo(DefaultIdType debitMemoId)
    {
        _voidCommand = new DebitMemoVoidCommand { DebitMemoId = debitMemoId, VoidReason = string.Empty };
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
            await Client.DebitMemoVoidEndpointAsync(_voidCommand.DebitMemoId, _voidCommand);
            Snackbar.Add("Debit memo voided successfully.", Severity.Success);
            _voidDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error voiding debit memo: {ex.Message}", Severity.Error);
        }
    }

    // View Applications
    private void OnViewApplications(DefaultIdType debitMemoId)
    {
        // Navigate to applications view or show dialog with application history
        Snackbar.Add("View applications feature coming soon.", Severity.Info);
    }
}

// Command objects for API calls
public record DebitMemoApproveCommand
{
    public DefaultIdType DebitMemoId { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTime ApprovedDate { get; set; }
}

public record DebitMemoApplyCommand
{
    public DefaultIdType DebitMemoId { get; set; }
    public DefaultIdType DocumentId { get; set; }
    public decimal AmountToApply { get; set; }
    public DateTime AppliedDate { get; set; }
}

public record DebitMemoVoidCommand
{
    public DefaultIdType DebitMemoId { get; set; }
    public string VoidReason { get; set; } = string.Empty;
}

// Search request extension (if not already defined in API client)
public class DebitMemoSearchRequest
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
