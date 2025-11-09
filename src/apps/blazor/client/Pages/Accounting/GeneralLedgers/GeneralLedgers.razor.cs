namespace FSH.Starter.Blazor.Client.Pages.Accounting.GeneralLedgers;

/// <summary>
/// General Ledger page for viewing and managing all financial transactions.
/// Provides comprehensive search, filtering, and drill-down capabilities.
/// </summary>
public partial class GeneralLedgers
{
    /// <summary>
    /// The entity table context for managing general ledger entries.
    /// </summary>
    protected EntityServerTableContext<GeneralLedgerSearchResponse, DefaultIdType, GeneralLedgerViewModel> Context { get; set; } = default!;

    /// <summary>
    /// Reference to the EntityTable component.
    /// </summary>
    private EntityTable<GeneralLedgerSearchResponse, DefaultIdType, GeneralLedgerViewModel> _table = default!;

    // Search filters
    private DefaultIdType? SearchAccountId { get; set; }
    private string? SearchReferenceNumber { get; set; }
    private DateTime? SearchStartDate { get; set; }
    private DateTime? SearchEndDate { get; set; }
    private DefaultIdType? SearchPeriodId { get; set; }
    private decimal? SearchMinDebit { get; set; }
    private decimal? SearchMaxDebit { get; set; }
    private decimal? SearchMinCredit { get; set; }
    private decimal? SearchMaxCredit { get; set; }
    private string? SearchUsoaClass { get; set; }

    // Dialog state
    private bool _showDetailsDialog;
    private DefaultIdType _selectedGeneralLedgerId;

    /// <summary>
    /// Initializes the component and sets up the entity table context.
    /// </summary>
    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<GeneralLedgerSearchResponse, DefaultIdType, GeneralLedgerViewModel>(
            entityName: "General Ledger Entry",
            entityNamePlural: "General Ledger Entries",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<GeneralLedgerSearchResponse>(response => response.TransactionDate, "Date", "TransactionDate", typeof(DateOnly)),
                new EntityField<GeneralLedgerSearchResponse>(response => response.AccountId, "Account", "AccountId"),
                new EntityField<GeneralLedgerSearchResponse>(response => response.ReferenceNumber, "Reference", "ReferenceNumber"),
                new EntityField<GeneralLedgerSearchResponse>(response => response.Debit, "Debit", "Debit", typeof(decimal)),
                new EntityField<GeneralLedgerSearchResponse>(response => response.Credit, "Credit", "Credit", typeof(decimal)),
                new EntityField<GeneralLedgerSearchResponse>(response => response.Memo, "Memo", "Memo"),
                new EntityField<GeneralLedgerSearchResponse>(response => response.UsoaClass, "USOA", "UsoaClass"),
                new EntityField<GeneralLedgerSearchResponse>(response => response.IsPosted, "Posted", "IsPosted", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var searchQuery = new GeneralLedgerSearchRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    AccountId = SearchAccountId,
                    PeriodId = SearchPeriodId,
                    UsoaClass = SearchUsoaClass,
                    StartDate = SearchStartDate,
                    EndDate = SearchEndDate,
                    MinDebit = SearchMinDebit,
                    MaxDebit = SearchMaxDebit,
                    MinCredit = SearchMinCredit,
                    MaxCredit = SearchMaxCredit,
                    ReferenceNumber = SearchReferenceNumber
                };

                var result = await Client.GeneralLedgerSearchEndpointAsync("1", searchQuery);
                return result.Adapt<PaginationResponse<GeneralLedgerSearchResponse>>();
            },
            getDetailsFunc: async id =>
            {
                var entry = await Client.GeneralLedgerGetEndpointAsync("1", id);
                return entry.Adapt<GeneralLedgerViewModel>();
            },
            updateFunc: async (id, viewModel) =>
            {
                if (viewModel.IsPosted)
                {
                    Snackbar.Add("Cannot update a posted general ledger entry. Use a reversing journal entry instead.", Severity.Error);
                    return;
                }

                var command = new GeneralLedgerUpdateCommand
                {
                    Id = id,
                    Debit = viewModel.Debit,
                    Credit = viewModel.Credit,
                    Memo = viewModel.Memo,
                    UsoaClass = viewModel.UsoaClass,
                    ReferenceNumber = viewModel.ReferenceNumber,
                    Description = viewModel.Description,
                    Notes = viewModel.Notes
                };

                await Client.GeneralLedgerUpdateEndpointAsync("1", id, command);
                Snackbar.Add("General Ledger entry updated successfully", Severity.Success);
            },
            hasExtraActionsFunc: () => true,
            canUpdateEntityFunc: entity => !entity.IsPosted,
            canDeleteEntityFunc: _ => false); // GL entries should never be deleted, only reversed

        base.OnInitialized();
    }

    /// <summary>
    /// Shows the details dialog for a general ledger entry.
    /// </summary>
    private void OnViewDetails(DefaultIdType id)
    {
        _selectedGeneralLedgerId = id;
        _showDetailsDialog = true;
        StateHasChanged();
    }

    /// <summary>
    /// Navigates to the source journal entry.
    /// </summary>
    private void OnViewJournalEntry(DefaultIdType entryId)
    {
        Navigation.NavigateTo($"/accounting/journal-entries?entryId={entryId}");
    }
}

