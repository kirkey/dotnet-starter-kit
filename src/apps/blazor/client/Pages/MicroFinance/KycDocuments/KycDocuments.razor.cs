namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.KycDocuments;

/// <summary>
/// KycDocuments page logic. Provides CRUD and search over KycDocument entities using the generated API client.
/// Manages KYC (Know Your Customer) documents for member verification and compliance.
/// </summary>
public partial class KycDocuments
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<KycDocumentResponse, DefaultIdType, KycDocumentViewModel> Context { get; set; } = null!;

    private EntityTable<KycDocumentResponse, DefaultIdType, KycDocumentViewModel> _table = null!;

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
    private bool _canVerify;
    private bool _canReject;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchDocumentNumber;
    private string? SearchDocumentNumber
    {
        get => _searchDocumentNumber;
        set
        {
            _searchDocumentNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchDocumentType;
    private string? SearchDocumentType
    {
        get => _searchDocumentType;
        set
        {
            _searchDocumentType = value;
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

    // Dialog state
    private bool _showRejectionDialog;
    private DefaultIdType _currentDocumentId;
    private string? _rejectionReason;

    /// <summary>
    /// Initializes the table context with KYC document-specific configuration.
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

        Context = new EntityServerTableContext<KycDocumentResponse, DefaultIdType, KycDocumentViewModel>(
            fields:
            [
                new EntityField<KycDocumentResponse>(dto => dto.DocumentType, "Type", "DocumentType"),
                new EntityField<KycDocumentResponse>(dto => dto.DocumentNumber, "Document #", "DocumentNumber"),
                new EntityField<KycDocumentResponse>(dto => dto.FileName, "File Name", "FileName"),
                new EntityField<KycDocumentResponse>(dto => dto.IssuingAuthority, "Issuing Authority", "IssuingAuthority"),
                new EntityField<KycDocumentResponse>(dto => dto.IssueDate, "Issue Date", "IssueDate", typeof(DateTimeOffset)),
                new EntityField<KycDocumentResponse>(dto => dto.ExpiryDate, "Expiry Date", "ExpiryDate", typeof(DateTimeOffset)),
                new EntityField<KycDocumentResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<KycDocumentResponse>(dto => dto.IsPrimary, "Primary", "IsPrimary", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                // Note: If SearchKycDocumentsCommand is not available, return empty result.
                // In production, this would be replaced with actual search endpoint when available.
                var response = new PaginationResponse<KycDocumentResponse>
                {
                    Items = new List<KycDocumentResponse>(),
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
                await Client.CreateKycDocumentAsync("1", viewModel.Adapt<CreateKycDocumentCommand>()).ConfigureAwait(false);
            },
            getDefaultsFunc: async () =>
            {
                return await Task.FromResult(new KycDocumentViewModel
                {
                    DocumentType = "NationalId",
                    IssueDate = DateTimeOffset.Now,
                    MimeType = "application/pdf"
                });
            },
            // KYC documents are typically not updated - they go through verify/reject workflow
            entityName: "KYC Document",
            entityNamePlural: "KYC Documents",
            entityResource: FshResources.Documents,
            hasExtraActionsFunc: () => _canVerify || _canReject);

        // Check permissions for extra actions
        var state = await AuthState;
        _canVerify = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.Documents);
        _canReject = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.Documents);
    }

    /// <summary>
    /// Show KYC documents help dialog.
    /// </summary>
    private async Task ShowKycDocumentsHelp()
    {
        await DialogService.ShowMessageBox(
            "KYC Documents Help",
            new MarkupString(@"
                <p><strong>KYC Documents</strong> (Know Your Customer) are identity verification documents uploaded by members.</p>
                <br/>
                <p><strong>Document Types:</strong></p>
                <ul>
                    <li><strong>National ID</strong> - Government-issued national ID card</li>
                    <li><strong>Passport</strong> - International passport</li>
                    <li><strong>Driver License</strong> - Valid driver's license</li>
                    <li><strong>Proof of Address</strong> - Utility bill, bank statement, etc.</li>
                    <li><strong>Photo</strong> - Passport-style photograph</li>
                </ul>
                <br/>
                <p><strong>Document Status:</strong></p>
                <ul>
                    <li><strong>Pending</strong> - Awaiting verification</li>
                    <li><strong>Verified</strong> - Document has been verified</li>
                    <li><strong>Rejected</strong> - Document was rejected (see reason)</li>
                    <li><strong>Expired</strong> - Document has expired</li>
                </ul>
                <br/>
                <p><strong>Workflow Actions:</strong></p>
                <ul>
                    <li><strong>Verify</strong> - Mark document as verified</li>
                    <li><strong>Reject</strong> - Reject document with reason</li>
                </ul>
            "));
    }

    /// <summary>
    /// Verify a KYC document.
    /// </summary>
    private async Task VerifyDocument(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Verify Document",
            "Are you sure you want to verify this KYC document?",
            yesText: "Verify",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.VerifyKycDocumentAsync("1", id, new VerifyKycDocumentRequest
                {
                    VerifiedById = DefaultIdType.Empty, // Current user ID would be set by the API
                    Notes = "Document verified through UI"
                }),
                successMessage: "Document verified successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Open the rejection dialog.
    /// </summary>
    private void OpenRejectDialog(DefaultIdType id)
    {
        _currentDocumentId = id;
        _rejectionReason = string.Empty;
        _showRejectionDialog = true;
    }

    /// <summary>
    /// Cancel the rejection dialog.
    /// </summary>
    private void CancelRejection()
    {
        _showRejectionDialog = false;
        _rejectionReason = string.Empty;
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
            () => Client.RejectKycDocumentAsync("1", _currentDocumentId, new RejectKycDocumentRequest
            {
                RejectedById = DefaultIdType.Empty, // Current user ID would be set by the API
                Reason = _rejectionReason
            }),
            successMessage: "Document rejected.");
        await _table.ReloadDataAsync();
    }

    /// <summary>
    /// View document details in a dialog.
    /// </summary>
    private async Task ViewDocumentDetails(DefaultIdType id)
    {
        try
        {
            var document = await Client.GetKycDocumentAsync("1", id);
            
            var statusColor = document.Status switch
            {
                "Verified" => "green",
                "Rejected" => "red",
                "Expired" => "orange",
                _ => "gray"
            };

            await DialogService.ShowMessageBox(
                $"KYC Document Details",
                new MarkupString($@"
                    <table style='width:100%'>
                        <tr><td><strong>Document Type:</strong></td><td>{document.DocumentType}</td></tr>
                        <tr><td><strong>Document Number:</strong></td><td>{document.DocumentNumber}</td></tr>
                        <tr><td><strong>Status:</strong></td><td><span style='color:{statusColor};font-weight:bold;'>{document.Status}</span></td></tr>
                        <tr><td><strong>Member ID:</strong></td><td>{document.MemberId}</td></tr>
                        <tr><td><strong>File Name:</strong></td><td>{document.FileName}</td></tr>
                        <tr><td><strong>File Path:</strong></td><td>{document.FilePath}</td></tr>
                        <tr><td><strong>MIME Type:</strong></td><td>{document.MimeType}</td></tr>
                        <tr><td><strong>File Size:</strong></td><td>{FormatFileSize(document.FileSize)}</td></tr>
                        <tr><td><strong>Issuing Authority:</strong></td><td>{document.IssuingAuthority ?? "N/A"}</td></tr>
                        <tr><td><strong>Issue Date:</strong></td><td>{document.IssueDate?.ToString("d") ?? "N/A"}</td></tr>
                        <tr><td><strong>Expiry Date:</strong></td><td>{document.ExpiryDate?.ToString("d") ?? "N/A"}</td></tr>
                        <tr><td><strong>Is Primary:</strong></td><td>{(document.IsPrimary ? "Yes" : "No")}</td></tr>
                        <tr><td><strong>Verified At:</strong></td><td>{document.VerifiedAt?.ToString("g") ?? "N/A"}</td></tr>
                        <tr><td><strong>Rejection Reason:</strong></td><td>{document.RejectionReason ?? "N/A"}</td></tr>
                    </table>
                "));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading document details: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Format file size for display.
    /// </summary>
    private static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}
