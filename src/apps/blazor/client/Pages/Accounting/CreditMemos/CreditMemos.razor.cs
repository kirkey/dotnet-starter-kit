namespace FSH.Starter.Blazor.Client.Pages.Accounting.CreditMemos;

public partial class CreditMemos
{
    protected EntityServerTableContext<CreditMemoResponse, DefaultIdType, CreditMemoViewModel> Context { get; set; } = default!;

    private EntityTable<CreditMemoResponse, DefaultIdType, CreditMemoViewModel> _table = default!;

    // Dialog visibility flags
    private bool _approveDialogVisible;
    private bool _applyDialogVisible;
    private bool _refundDialogVisible;
    private bool _voidDialogVisible;

    // Dialog options
    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

    // Command objects for dialogs - using API-generated command classes
    private DefaultIdType _currentMemoId = DefaultIdType.Empty;
    private ApproveCreditMemoCommand _approveCommand = new() { ApprovedBy = string.Empty };
    private ApplyCreditMemoCommand _applyCommand = new() { AmountToApply = 0 };
    private RefundCreditMemoCommand _refundCommand = new() { RefundAmount = 0, RefundMethod = string.Empty };
    private VoidCreditMemoCommand _voidCommand = new();

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
        Context = new EntityServerTableContext<CreditMemoResponse, DefaultIdType, CreditMemoViewModel>(
            entityName: "Credit Memo",
            entityNamePlural: "Credit Memos",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<CreditMemoResponse>(response => response.MemoNumber, "Memo Number", "MemoNumber"),
                new EntityField<CreditMemoResponse>(response => response.MemoDate, "Date", "MemoDate", typeof(DateOnly)),
                new EntityField<CreditMemoResponse>(response => response.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<CreditMemoResponse>(response => response.AppliedAmount, "Applied", "AppliedAmount", typeof(decimal)),
                new EntityField<CreditMemoResponse>(response => response.RefundedAmount, "Refunded", "RefundedAmount", typeof(decimal)),
                new EntityField<CreditMemoResponse>(response => response.UnappliedAmount, "Unapplied", "UnappliedAmount", typeof(decimal)),
                new EntityField<CreditMemoResponse>(response => response.ReferenceType, "Type", "ReferenceType"),
                new EntityField<CreditMemoResponse>(response => response.Status, "Status", "Status"),
                new EntityField<CreditMemoResponse>(response => response.ApprovalStatus, "Approval", "ApprovalStatus"),
                new EntityField<CreditMemoResponse>(response => response.Reason, "Reason", "Reason"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchCreditMemosQuery>();
                var result = await Client.CreditMemoSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<CreditMemoResponse>>();
            },
            createFunc: async creditMemo =>
            {
                await Client.CreditMemoCreateEndpointAsync("1", creditMemo.Adapt<CreateCreditMemoCommand>());
            },
            updateFunc: async (id, creditMemo) =>
            {
                await Client.CreditMemoUpdateEndpointAsync("1", id, creditMemo.Adapt<UpdateCreditMemoCommand>());
            },
            deleteFunc: async id =>
            {
                // Only allow deletion of Draft memos
                var memoDetails = await Client.CreditMemoGetEndpointAsync("1", id);
                if (memoDetails.Status != "Draft")
                {
                    Snackbar.Add("Only draft credit memos can be deleted.", Severity.Warning);
                    return;
                }
                await Client.CreditMemoDeleteEndpointAsync("1", id);
            },
            // getDetailsFunc: async id =>
            // {
            //     var response = await Client.CreditMemoGetEndpointAsync("1", id);
            //     return response.Adapt<CreditMemoViewModel>();
            // },
            hasExtraActionsFunc: () => true);

        return Task.CompletedTask;
    }

    // Approve Credit Memo Dialog
    private void OnApproveMemo(DefaultIdType creditMemoId)
    {
        _currentMemoId = creditMemoId;
        _approveCommand = new ApproveCreditMemoCommand 
        { 
            ApprovedBy = string.Empty
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
            await Client.CreditMemoApproveEndpointAsync("1", _currentMemoId, _approveCommand);
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
        _currentMemoId = creditMemoId;
        _applyCommand = new ApplyCreditMemoCommand 
        { 
            AmountToApply = 0
        };
        _applyDialogVisible = true;
    }

    private async Task SubmitApplyMemo()
    {
        if (_applyCommand.AmountToApply <= 0)
        {
            Snackbar.Add("Amount to apply must be greater than zero.", Severity.Error);
            return;
        }

        try
        {
            await Client.CreditMemoApplyEndpointAsync("1", _currentMemoId, _applyCommand);
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
        _currentMemoId = creditMemoId;
        _refundCommand = new RefundCreditMemoCommand 
        { 
            RefundAmount = 0, 
            RefundMethod = string.Empty
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
            await Client.CreditMemoRefundEndpointAsync("1", _currentMemoId, _refundCommand);
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
        _currentMemoId = creditMemoId;
        _voidCommand = new VoidCreditMemoCommand { VoidReason = string.Empty };
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
            await Client.CreditMemoVoidEndpointAsync("1", _currentMemoId, _voidCommand);
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
