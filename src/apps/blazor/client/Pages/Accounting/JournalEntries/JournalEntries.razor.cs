namespace FSH.Starter.Blazor.Client.Pages.Accounting.JournalEntries;

/// <summary>
/// Journal Entries page for managing double-entry accounting transactions.
/// </summary>
public partial class JournalEntries
{
    /// <summary>
    /// The entity table context for managing journal entries with server-side operations.
    /// </summary>
    protected EntityServerTableContext<JournalEntryResponse, DefaultIdType, JournalEntryViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component for journal entries.
    /// </summary>
    private EntityTable<JournalEntryResponse, DefaultIdType, JournalEntryViewModel> _table = null!;

    /// <summary>
    /// Search filter for reference number.
    /// </summary>
    private string? ReferenceNumber { get; set; }

    /// <summary>
    /// Search filter for source system.
    /// </summary>
    private string? Source { get; set; }

    /// <summary>
    /// Search filter for date range start.
    /// </summary>
    private DateTime? FromDate { get; set; }

    /// <summary>
    /// Search filter for date range end.
    /// </summary>
    private DateTime? ToDate { get; set; }

    /// <summary>
    /// Search filter for posted status.
    /// </summary>
    private bool? IsPosted { get; set; }

    /// <summary>
    /// Search filter for approval status.
    /// </summary>
    private string? ApprovalStatus { get; set; }

    /// <summary>
    /// Dialog options for modal dialogs.
    /// </summary>
    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true };

    /// <summary>
    /// Gets the status color based on approval status.
    /// </summary>
    private static Color GetStatusColor(string? status) => status switch
    {
        "Pending" => Color.Warning,
        "Approved" => Color.Info,
        "Rejected" => Color.Error,
        _ => Color.Default
    };

    /// <summary>
    /// Gets the severity for balance indicator.
    /// </summary>
    private static Severity GetBalanceSeverity(JournalEntryViewModel model) =>
        model.IsBalanced ? Severity.Success : Severity.Warning;

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<JournalEntryResponse, DefaultIdType, JournalEntryViewModel>(
            entityName: "Journal Entry",
            entityNamePlural: "Journal Entries",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<JournalEntryResponse>(response => response.Date, "Date", "Date", typeof(DateOnly)),
                new EntityField<JournalEntryResponse>(response => response.ReferenceNumber, "Reference", "ReferenceNumber"),
                new EntityField<JournalEntryResponse>(response => response.Source, "Source", "Source"),
                new EntityField<JournalEntryResponse>(response => response.TotalDebits, "Debits", "TotalDebits", typeof(decimal)),
                new EntityField<JournalEntryResponse>(response => response.TotalCredits, "Credits", "TotalCredits", typeof(decimal)),
                new EntityField<JournalEntryResponse>(response => response.ApprovalStatus, "Approval", "ApprovalStatus"),
                new EntityField<JournalEntryResponse>(response => response.IsPosted, "Posted", "IsPosted", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchJournalEntriesRequest
                {
                    ReferenceNumber = ReferenceNumber,
                    Source = Source,
                    FromDate = FromDate,
                    ToDate = ToDate,
                    IsPosted = IsPosted,
                    ApprovalStatus = ApprovalStatus,
                    
                    
                    
                    
                };

                var result = await Client.JournalEntrySearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<JournalEntryResponse>>();
            },
            createFunc: async viewModel =>
            {
                if (!viewModel.IsBalanced)
                {
                    Snackbar.Add("Journal entry must be balanced before saving.", Severity.Error);
                    return;
                }

                var command = new CreateJournalEntryCommand
                {
                    Date = viewModel.Date.GetValueOrDefault(DateTime.Today),
                    ReferenceNumber = viewModel.ReferenceNumber,
                    Source = viewModel.Source,
                    Description = viewModel.Description,
                    Lines = [.. viewModel.Lines.Select(l => new JournalEntryLineDto
                    {
                        AccountId = (DefaultIdType)l.AccountId!,
                        DebitAmount = l.DebitAmount,
                        CreditAmount = l.CreditAmount,
                        Description = l.Description
                    })],
                    PeriodId = viewModel.PeriodId,
                    OriginalAmount = viewModel.OriginalAmount,
                    Notes = viewModel.Notes
                };

                await Client.JournalEntryCreateEndpointAsync("1", command);
                Snackbar.Add("Journal Entry created successfully", Severity.Success);
            },
            updateFunc: async (id, viewModel) =>
            {
                if (!viewModel.IsBalanced)
                {
                    Snackbar.Add("Journal entry must be balanced before saving.", Severity.Error);
                    return;
                }

                if (viewModel.IsPosted)
                {
                    Snackbar.Add("Cannot update a posted journal entry. Use reverse instead.", Severity.Error);
                    return;
                }

                var command = new UpdateJournalEntryCommand
                {
                    Id = id,
                    ReferenceNumber = viewModel.ReferenceNumber,
                    Date = viewModel.Date,
                    Source = viewModel.Source,
                    PeriodId = viewModel.PeriodId,
                    OriginalAmount = viewModel.OriginalAmount,
                    Description = viewModel.Description,
                    Notes = viewModel.Notes
                };

                await Client.JournalEntryUpdateEndpointAsync("1", id, command);
                Snackbar.Add("Journal Entry updated successfully", Severity.Success);
            },
            deleteFunc: async id =>
            {
                await Client.JournalEntryDeleteEndpointAsync("1", id);
                Snackbar.Add("Journal Entry deleted successfully", Severity.Success);
            },
            // getDetailsFunc: async id =>
            // {
            //     var entry = await Client.JournalEntryGetEndpointAsync("1", id);
            //     return MapToViewModel(entry);
            // },
            getDefaultsFunc: () => Task.FromResult(new JournalEntryViewModel
            {
                Date = DateTime.Today,
                Source = "ManualEntry",
                Lines = []
            }));

        return base.OnInitializedAsync();
    }

    /// <summary>
    /// Maps a JournalEntryResponse to JournalEntryViewModel.
    /// </summary>
    private static JournalEntryViewModel MapToViewModel(JournalEntryResponse response)
    {
        return new JournalEntryViewModel
        {
            Id = response.Id,
            Date = response.Date,
            ReferenceNumber = response.ReferenceNumber ?? string.Empty,
            Source = response.Source ?? "ManualEntry",
            Description = response.Description ?? string.Empty,
            PeriodId = response.PeriodId,
            OriginalAmount = response.OriginalAmount,
            Notes = response.Notes,
            IsPosted = response.IsPosted,
            ApprovalStatus = response.ApprovalStatus ?? "Pending",
            ApprovedBy = response.ApprovedBy,
            ApprovedDate = response.ApprovedDate,
            Lines = response.Lines?.Select(l => new JournalEntryLineViewModel
            {
                Id = l.Id,
                AccountId = l.AccountId,
                AccountCode = string.Empty, // Will be loaded by autocomplete
                AccountName = string.Empty,
                DebitAmount = l.DebitAmount,
                CreditAmount = l.CreditAmount,
                Description = l.Memo // Note: API uses 'Memo' but UI uses 'Description'
            }).ToList() ?? []
        };
    }

    /// <summary>
    /// Posts a journal entry to the general ledger.
    /// </summary>
    private async Task OnPost(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Confirm Post",
            "Are you sure you want to post this journal entry to the general ledger? This action cannot be undone.",
            yesText: "Post", cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                await Client.JournalEntryPostEndpointAsync("1", id);
                Snackbar.Add("Journal entry posted successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error posting journal entry: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Reverses a posted journal entry.
    /// </summary>
    private async Task OnReverse(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(ReverseJournalEntryDialog.JournalEntryId), id }
        };

        var dialog = await DialogService.ShowAsync<ReverseJournalEntryDialog>(
            "Reverse Journal Entry", parameters, _dialogOptions);

        if (dialog != null)
        {
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await _table.ReloadDataAsync();
            }
        }
    }

    /// <summary>
    /// Approves a pending journal entry.
    /// </summary>
    private async Task OnApprove(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Confirm Approval",
            "Are you sure you want to approve this journal entry?",
            yesText: "Approve", cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                var request = new ApproveJournalEntryRequest
                {
                    ApprovedBy = "CurrentUser" // TODO: Get actual user
                };
                await Client.JournalEntryApproveEndpointAsync("1", id, request);
                Snackbar.Add("Journal entry approved successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error approving journal entry: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Opens the details dialog for a journal entry.
    /// </summary>
    private async Task OnViewDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(JournalEntryDetailsDialog.JournalEntryId), id }
        };

        var options = new DialogOptions 
        { 
            CloseOnEscapeKey = true, 
            MaxWidth = MaxWidth.ExtraLarge, 
            FullWidth = true 
        };

        await DialogService.ShowAsync<JournalEntryDetailsDialog>(
            "Journal Entry Details", parameters, options);
    }

    /// <summary>
    /// Rejects a pending journal entry.
    /// </summary>
    private async Task OnReject(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(RejectJournalEntryDialog.JournalEntryId), id }
        };

        var dialog = await DialogService.ShowAsync<RejectJournalEntryDialog>(
            "Reject Journal Entry", parameters, _dialogOptions);

        if (dialog != null)
        {
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await _table.ReloadDataAsync();
            }
        }
    }
}
