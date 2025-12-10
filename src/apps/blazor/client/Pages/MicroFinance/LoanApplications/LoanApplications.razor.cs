namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanApplications;

/// <summary>
/// LoanApplications page logic. Provides CRUD and search over LoanApplication entities using the generated API client.
/// Manages loan applications including submissions, reviews, approvals, rejections, and assignments.
/// </summary>
public partial class LoanApplications
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<LoanApplicationResponse, DefaultIdType, LoanApplicationViewModel> Context { get; set; } = null!;

    private EntityTable<LoanApplicationResponse, DefaultIdType, LoanApplicationViewModel> _table = null!;

    /// <summary>
    /// Authorization state for permission checks.
    /// </summary>
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    /// <summary>
    /// Authorization service for permission checks.
    /// </summary>
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    // Permission flags
    private bool _canSubmit;
    private bool _canAssign;
    private bool _canApprove;
    private bool _canReject;
    private bool _canReturn;
    private bool _canWithdraw;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchApplicationNumber;
    private string? SearchApplicationNumber
    {
        get => _searchApplicationNumber;
        set
        {
            _searchApplicationNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchStatus;
    private string? SearchStatus
    {
        get => _searchStatus;
        set
        {
            _searchStatus = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _searchApplicationDateFrom;
    private DateTime? SearchApplicationDateFrom
    {
        get => _searchApplicationDateFrom;
        set
        {
            _searchApplicationDateFrom = value;
            _ = _table.ReloadDataAsync();
        }
    }

    // Dialog state
    private bool _showApprovalDialog;
    private bool _showRejectionDialog;
    private bool _showReturnDialog;
    private bool _showAssignDialog;

    // Approval dialog fields
    private DefaultIdType _currentApplicationId;
    private decimal _approvalAmount;
    private int _approvalTermMonths = 12;
    private decimal _approvalInterestRate = 18.5m;
    private string? _approvalConditions;

    // Rejection dialog fields
    private string? _rejectionReason;

    // Return dialog fields
    private string? _returnReason;
    private string? _returnNotes;

    // Assign dialog fields
    private string? _assignOfficerId;

    /// <summary>
    /// Initializes the table context with loan application-specific configuration.
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

        Context = new EntityServerTableContext<LoanApplicationResponse, DefaultIdType, LoanApplicationViewModel>(
            fields:
            [
                new EntityField<LoanApplicationResponse>(dto => dto.ApplicationNumber, "Application #", "ApplicationNumber"),
                new EntityField<LoanApplicationResponse>(dto => dto.MemberId, "Member ID", "MemberId"),
                new EntityField<LoanApplicationResponse>(dto => dto.LoanProductId, "Product ID", "LoanProductId"),
                new EntityField<LoanApplicationResponse>(dto => dto.RequestedAmount, "Requested", "RequestedAmount", typeof(decimal)),
                new EntityField<LoanApplicationResponse>(dto => dto.ApprovedAmount, "Approved", "ApprovedAmount", typeof(decimal)),
                new EntityField<LoanApplicationResponse>(dto => dto.RequestedTermMonths, "Term (Mo)", "RequestedTermMonths", typeof(int)),
                new EntityField<LoanApplicationResponse>(dto => dto.Purpose, "Purpose", "Purpose"),
                new EntityField<LoanApplicationResponse>(dto => dto.ApplicationDate, "Applied", "ApplicationDate", typeof(DateTimeOffset)),
                new EntityField<LoanApplicationResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                // Note: If SearchLoanApplicationsCommand is not available, you may need to create a custom search endpoint
                // or use a generic list endpoint. For now, we'll use GetLoanApplicationAsync for individual lookups
                // and simulate pagination. Replace with actual search when available.
                
                // Placeholder: Return empty result until search endpoint is available
                // In a real implementation, you would call Client.SearchLoanApplicationsAsync()
                var response = new PaginationResponse<LoanApplicationResponse>
                {
                    Items = new List<LoanApplicationResponse>(),
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalCount = 0
                };
                return await Task.FromResult(response);
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                viewModel.SyncIdsFromSelections();
                await Client.CreateLoanApplicationAsync("1", viewModel.Adapt<CreateLoanApplicationCommand>()).ConfigureAwait(false);
            },
            getDefaultsFunc: async () =>
            {
                return await Task.FromResult(new LoanApplicationViewModel
                {
                    RequestedAmount = 100000m,
                    RequestedTermMonths = 12,
                    Purpose = "Business Expansion"
                });
            },
            // Loan applications are typically not updated directly - they go through workflow actions
            // updateFunc: null means no update capability through the standard edit form
            entityName: "Loan Application",
            entityNamePlural: "Loan Applications",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canSubmit || _canAssign || _canApprove || _canReject || _canReturn || _canWithdraw);

        // Check permissions for extra actions
        var state = await AuthState;
        _canSubmit = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
        _canAssign = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
        _canApprove = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
        _canReject = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
        _canReturn = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
        _canWithdraw = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show loan applications help dialog.
    /// </summary>
    private async Task ShowLoanApplicationsHelp()
    {
        await DialogService.ShowMessageBox(
            "Loan Applications Help",
            new MarkupString(@"
                <p><strong>Loan Applications</strong> manage the loan request workflow from submission to approval.</p>
                <br/>
                <p><strong>Workflow States:</strong></p>
                <ul>
                    <li><strong>Draft</strong> - Application created but not submitted</li>
                    <li><strong>Submitted</strong> - Application submitted for review</li>
                    <li><strong>Under Review</strong> - Application assigned to an officer</li>
                    <li><strong>Approved</strong> - Application approved, ready for disbursement</li>
                    <li><strong>Rejected</strong> - Application rejected</li>
                    <li><strong>Returned</strong> - Application returned for revision</li>
                    <li><strong>Withdrawn</strong> - Application withdrawn by applicant</li>
                    <li><strong>Disbursed</strong> - Loan has been disbursed</li>
                </ul>
                <br/>
                <p><strong>Actions:</strong></p>
                <ul>
                    <li><strong>Submit</strong> - Submit draft application for review</li>
                    <li><strong>Assign</strong> - Assign application to a loan officer</li>
                    <li><strong>Approve</strong> - Approve the application with terms</li>
                    <li><strong>Reject</strong> - Reject the application with reason</li>
                    <li><strong>Return</strong> - Return for revision with notes</li>
                    <li><strong>Withdraw</strong> - Withdraw the application</li>
                </ul>
            "));
    }

    /// <summary>
    /// View loan application details.
    /// </summary>
    private async Task ViewApplicationDetails(DefaultIdType id)
    {
        try
        {
            var application = await Client.GetLoanApplicationAsync("1", id);
            await DialogService.ShowMessageBox(
                $"Application Details - {application.ApplicationNumber}",
                new MarkupString($@"
                    <table style='width:100%'>
                        <tr><td><strong>Application #:</strong></td><td>{application.ApplicationNumber}</td></tr>
                        <tr><td><strong>Status:</strong></td><td>{application.Status}</td></tr>
                        <tr><td><strong>Member ID:</strong></td><td>{application.MemberId}</td></tr>
                        <tr><td><strong>Product ID:</strong></td><td>{application.LoanProductId}</td></tr>
                        <tr><td><strong>Requested Amount:</strong></td><td>{application.RequestedAmount:C}</td></tr>
                        <tr><td><strong>Approved Amount:</strong></td><td>{application.ApprovedAmount?.ToString("C") ?? "N/A"}</td></tr>
                        <tr><td><strong>Requested Term:</strong></td><td>{application.RequestedTermMonths} months</td></tr>
                        <tr><td><strong>Approved Term:</strong></td><td>{application.ApprovedTermMonths?.ToString() ?? "N/A"} months</td></tr>
                        <tr><td><strong>Purpose:</strong></td><td>{application.Purpose}</td></tr>
                        <tr><td><strong>Application Date:</strong></td><td>{application.ApplicationDate:d}</td></tr>
                        <tr><td><strong>Decision Date:</strong></td><td>{application.DecisionAt?.ToString("d") ?? "N/A"}</td></tr>
                        <tr><td><strong>Rejection Reason:</strong></td><td>{application.RejectionReason ?? "N/A"}</td></tr>
                    </table>
                "));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading application details: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Submit a draft application.
    /// </summary>
    private async Task SubmitApplication(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Submit Application",
            "Are you sure you want to submit this loan application for review?",
            yesText: "Submit",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.SubmitLoanApplicationAsync("1", id),
                successMessage: "Application submitted successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Open the assign officer dialog.
    /// </summary>
    private Task AssignApplication(DefaultIdType id)
    {
        _currentApplicationId = id;
        _assignOfficerId = string.Empty;
        _showAssignDialog = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Confirm officer assignment.
    /// </summary>
    private async Task ConfirmAssign()
    {
        if (string.IsNullOrWhiteSpace(_assignOfficerId) || !DefaultIdType.TryParse(_assignOfficerId, out var officerId))
        {
            Snackbar.Add("Please enter a valid Officer ID.", Severity.Warning);
            return;
        }

        _showAssignDialog = false;

        await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.AssignLoanApplicationAsync("1", _currentApplicationId, new AssignLoanApplicationCommand
            {
                Id = _currentApplicationId,
                OfficerId = officerId
            }),
            successMessage: "Application assigned to officer successfully.");
        await _table.ReloadDataAsync();
    }

    /// <summary>
    /// Open the approval dialog.
    /// </summary>
    private async Task ApproveApplication(DefaultIdType id)
    {
        try
        {
            var application = await Client.GetLoanApplicationAsync("1", id);
            _currentApplicationId = id;
            _approvalAmount = application.RequestedAmount;
            _approvalTermMonths = application.RequestedTermMonths;
            _approvalInterestRate = 18.5m;
            _approvalConditions = string.Empty;
            _showApprovalDialog = true;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading application: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Confirm approval.
    /// </summary>
    private async Task ConfirmApproval()
    {
        _showApprovalDialog = false;

        await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.ApproveLoanApplicationAsync("1", _currentApplicationId, new ApproveLoanApplicationCommand
            {
                Id = _currentApplicationId,
                ApprovedAmount = _approvalAmount,
                ApprovedTermMonths = _approvalTermMonths,
                ApprovedInterestRate = _approvalInterestRate,
                ApprovalConditions = _approvalConditions
            }),
            successMessage: "Application approved successfully.");
        await _table.ReloadDataAsync();
    }

    /// <summary>
    /// Open the rejection dialog.
    /// </summary>
    private Task RejectApplication(DefaultIdType id)
    {
        _currentApplicationId = id;
        _rejectionReason = string.Empty;
        _showRejectionDialog = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Confirm rejection.
    /// </summary>
    private async Task ConfirmRejection()
    {
        if (string.IsNullOrWhiteSpace(_rejectionReason))
        {
            Snackbar.Add("Please provide a rejection reason.", Severity.Warning);
            return;
        }

        _showRejectionDialog = false;

        await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.RejectLoanApplicationAsync("1", _currentApplicationId, new RejectLoanApplicationCommand
            {
                Id = _currentApplicationId,
                Reason = _rejectionReason
            }),
            successMessage: "Application rejected.");
        await _table.ReloadDataAsync();
    }

    /// <summary>
    /// Open the return dialog.
    /// </summary>
    private Task ReturnApplication(DefaultIdType id)
    {
        _currentApplicationId = id;
        _returnReason = string.Empty;
        _returnNotes = string.Empty;
        _showReturnDialog = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Confirm return for revision.
    /// </summary>
    private async Task ConfirmReturn()
    {
        if (string.IsNullOrWhiteSpace(_returnReason))
        {
            Snackbar.Add("Please provide a reason for returning the application.", Severity.Warning);
            return;
        }

        _showReturnDialog = false;

        await ApiHelper.ExecuteCallGuardedAsync(
            () => Client.ReturnLoanApplicationAsync("1", _currentApplicationId, new ReturnLoanApplicationCommand
            {
                LoanApplicationId = _currentApplicationId,
                Reason = _returnReason,
                Notes = _returnNotes
            }),
            successMessage: "Application returned for revision.");
        await _table.ReloadDataAsync();
    }

    /// <summary>
    /// Withdraw an application.
    /// </summary>
    private async Task WithdrawApplication(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Withdraw Application",
            "Are you sure you want to withdraw this loan application? This action cannot be undone.",
            yesText: "Withdraw",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.WithdrawLoanApplicationAsync("1", id),
                successMessage: "Application withdrawn.");
            await _table.ReloadDataAsync();
        }
    }
}
