namespace FSH.Starter.Blazor.Client.Pages.Accounting.DebitMemos;

public partial class DebitMemos
{
    protected EntityServerTableContext<DebitMemoResponse, DefaultIdType, DebitMemoViewModel> Context { get; set; } = null!;

    private EntityTable<DebitMemoResponse, DefaultIdType, DebitMemoViewModel> _table = null!;

    // Dialog visibility flags
    private bool _approveDialogVisible;
    private bool _applyDialogVisible;
    private bool _voidDialogVisible;

    // Dialog options
    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

    // Command objects for dialogs - using API-generated command classes
    private DefaultIdType _currentMemoId = DefaultIdType.Empty;
    private ApproveDebitMemoCommand _approveCommand = new() { ApprovedBy = string.Empty };
    private ApplyDebitMemoCommand _applyCommand = new() { AmountToApply = 0 };
    private VoidDebitMemoCommand _voidCommand = new();

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
        Context = new EntityServerTableContext<DebitMemoResponse, DefaultIdType, DebitMemoViewModel>(
            entityName: "Debit Memo",
            entityNamePlural: "Debit Memos",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<DebitMemoResponse>(response => response.MemoNumber, "Memo Number", "MemoNumber"),
                new EntityField<DebitMemoResponse>(response => response.MemoDate, "Date", "MemoDate", typeof(DateOnly)),
                new EntityField<DebitMemoResponse>(response => response.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<DebitMemoResponse>(response => response.AppliedAmount, "Applied", "AppliedAmount", typeof(decimal)),
                new EntityField<DebitMemoResponse>(response => response.UnappliedAmount, "Unapplied", "UnappliedAmount", typeof(decimal)),
                new EntityField<DebitMemoResponse>(response => response.ReferenceType, "Type", "ReferenceType"),
                new EntityField<DebitMemoResponse>(response => response.Status, "Status", "Status"),
                new EntityField<DebitMemoResponse>(response => response.ApprovalStatus, "Approval", "ApprovalStatus"),
                new EntityField<DebitMemoResponse>(response => response.Reason, "Reason", "Reason"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchDebitMemosQuery
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.DebitMemoSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<DebitMemoResponse>>();
            },
            createFunc: async debitMemo =>
            {
                await Client.DebitMemoCreateEndpointAsync("1", debitMemo.Adapt<CreateDebitMemoCommand>());
            },
            updateFunc: async (id, debitMemo) =>
            {
                var updateCommand = new UpdateDebitMemoCommand
                {
                    Id = id,
                    MemoDate = debitMemo.MemoDate,
                    Amount = debitMemo.Amount,
                    Reason = debitMemo.Reason,
                    Description = debitMemo.Description,
                    Notes = debitMemo.Notes
                };
                await Client.DebitMemoUpdateEndpointAsync("1", id, updateCommand);
            },
            deleteFunc: async id =>
            {
                // Only allow deletion of Draft memos
                var memoDetails = await Client.DebitMemoGetEndpointAsync("1", id);
                if (memoDetails.Status != "Draft")
                {
                    Snackbar.Add("Only draft debit memos can be deleted.", Severity.Warning);
                    return;
                }
                await Client.DebitMemoDeleteEndpointAsync("1", id);
            },
            // getDetailsFunc: async id =>
            // {
            //     var response = await Client.DebitMemoGetEndpointAsync("1", id);
            //     return response.Adapt<DebitMemoViewModel>();
            // },
            hasExtraActionsFunc: () => true);

        return Task.CompletedTask;
    }

    // Approve Debit Memo Dialog
    private void OnApproveMemo(DefaultIdType debitMemoId)
    {
        _currentMemoId = debitMemoId;
        _approveCommand = new ApproveDebitMemoCommand 
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
            await Client.DebitMemoApproveEndpointAsync("1", _currentMemoId, _approveCommand);
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
        _currentMemoId = debitMemoId;
        _applyCommand = new ApplyDebitMemoCommand 
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
            await Client.DebitMemoApplyEndpointAsync("1", _currentMemoId, _applyCommand);
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
        _currentMemoId = debitMemoId;
        _voidCommand = new VoidDebitMemoCommand();
        _voidDialogVisible = true;
    }

    private async Task SubmitVoidMemo()
    {
        try
        {
            await Client.DebitMemoVoidEndpointAsync("1", _currentMemoId, _voidCommand);
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
