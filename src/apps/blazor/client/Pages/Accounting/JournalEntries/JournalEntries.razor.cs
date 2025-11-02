using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Shared;
using FSH.Starter.WebApi.Accounting.JournalEntries;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.JournalEntries;

/// <summary>
/// Journal Entries page for managing double-entry accounting transactions.
/// </summary>
public partial class JournalEntries
{
    [Inject] private IAccountingClient AccountingClient { get; set; } = default!;

    protected EntityServerTableContext<JournalEntryResponse, DefaultIdType, JournalEntryViewModel> Context { get; set; } = default!;

    private EntityTable<JournalEntryResponse, DefaultIdType, JournalEntryViewModel> _table = default!;

    // Search filters
    private string? ReferenceNumber { get; set; }
    private string? Source { get; set; }
    private DateTime? FromDate { get; set; }
    private DateTime? ToDate { get; set; }
    private bool? IsPosted { get; set; }
    private string? ApprovalStatus { get; set; }

    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

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

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<JournalEntryResponse, DefaultIdType, JournalEntryViewModel>(
            entityName: "Journal Entry",
            entityNamePlural: "Journal Entries",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<JournalEntryResponse>(r => r.Date, "Date", "Date", typeof(DateTime)),
                new EntityField<JournalEntryResponse>(r => r.ReferenceNumber, "Reference", "ReferenceNumber"),
                new EntityField<JournalEntryResponse>(r => r.Source, "Source", "Source"),
                new EntityField<JournalEntryResponse>(r => r.TotalDebits, "Debits", "TotalDebits", typeof(decimal)),
                new EntityField<JournalEntryResponse>(r => r.TotalCredits, "Credits", "TotalCredits", typeof(decimal)),
                new EntityField<JournalEntryResponse>(r => r.ApprovalStatus, "Approval", "ApprovalStatus"),
                new EntityField<JournalEntryResponse>(r => r.IsPosted, "Posted", "IsPosted", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var searchQuery = new SearchJournalEntriesQuery
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    OrderBy = filter.OrderBy,
                    Keyword = filter.Keyword,
                    ReferenceNumber = ReferenceNumber,
                    Source = Source,
                    FromDate = FromDate,
                    ToDate = ToDate,
                    IsPosted = IsPosted,
                    ApprovalStatus = ApprovalStatus
                };

                var result = await AccountingClient.SearchJournalEntriesAsync(searchQuery);
                return result.Adapt<PaginationResponse<JournalEntryResponse>>();
            },
            createFunc: async model =>
            {
                if (!model.IsBalanced)
                {
                    Snackbar.Add("Journal entry must be balanced before saving.", Severity.Error);
                    return;
                }

                var command = new CreateJournalEntryCommand(
                    model.Date.GetValueOrDefault(DateTime.Today),
                    model.ReferenceNumber,
                    model.Source,
                    model.Description,
                    model.Lines.Select(l => new JournalEntryLineDto(
                        l.AccountId,
                        l.DebitAmount,
                        l.CreditAmount,
                        l.Description
                    )).ToList(),
                    model.PeriodId,
                    model.OriginalAmount,
                    model.Notes
                );

                await AccountingClient.CreateJournalEntryAsync(command);
                Snackbar.Add("Journal Entry created successfully", Severity.Success);
            },
            updateFunc: async (id, model) =>
            {
                if (!model.IsBalanced)
                {
                    Snackbar.Add("Journal entry must be balanced before saving.", Severity.Error);
                    return;
                }

                if (model.IsPosted)
                {
                    Snackbar.Add("Cannot update a posted journal entry. Use reverse instead.", Severity.Error);
                    return;
                }

                var command = new UpdateJournalEntryCommand(
                    id,
                    model.ReferenceNumber,
                    model.Date,
                    model.Source,
                    model.PeriodId,
                    model.OriginalAmount,
                    model.Description,
                    model.Notes
                );

                await AccountingClient.UpdateJournalEntryAsync(id, command);
                Snackbar.Add("Journal Entry updated successfully", Severity.Success);
            },
            deleteFunc: async id =>
            {
                await AccountingClient.DeleteJournalEntryAsync(id);
                Snackbar.Add("Journal Entry deleted successfully", Severity.Success);
            },
            getDetailsFunc: async id =>
            {
                var entry = await AccountingClient.GetJournalEntryAsync(id);
                return MapToViewModel(entry);
            },
            getDefaultsFunc: () => Task.FromResult(new JournalEntryViewModel
            {
                Date = DateTime.Today,
                Source = "ManualEntry",
                Lines = new List<JournalEntryLineViewModel>()
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
            ReferenceNumber = response.ReferenceNumber,
            Source = response.Source,
            Description = response.Description,
            PeriodId = response.PeriodId,
            OriginalAmount = response.OriginalAmount,
            Notes = response.Notes,
            IsPosted = response.IsPosted,
            ApprovalStatus = response.ApprovalStatus,
            ApprovedBy = response.ApprovedBy,
            ApprovedDate = response.ApprovedDate,
            Lines = response.Lines.Select(l => new JournalEntryLineViewModel
            {
                Id = l.Id,
                AccountId = l.AccountId,
                AccountCode = string.Empty, // Will be loaded by autocomplete
                AccountName = string.Empty,
                DebitAmount = l.DebitAmount,
                CreditAmount = l.CreditAmount,
                Description = l.Description
            }).ToList()
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
                await AccountingClient.PostJournalEntryAsync(id);
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

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
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
                var request = new ApproveJournalEntryRequest("CurrentUser"); // TODO: Get actual user
                await AccountingClient.ApproveJournalEntryAsync(id, request);
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

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }
}
