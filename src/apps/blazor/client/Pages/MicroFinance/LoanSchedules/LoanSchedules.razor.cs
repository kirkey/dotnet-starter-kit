namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanSchedules;

/// <summary>
/// LoanSchedules page logic. Provides search and view operations for LoanSchedule entities.
/// This is a read-only view - schedules are generated automatically when loans are disbursed.
/// </summary>
public partial class LoanSchedules
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<LoanScheduleResponse, DefaultIdType, LoanScheduleViewModel> Context { get; set; } = null!;

    private EntityTable<LoanScheduleResponse, DefaultIdType, LoanScheduleViewModel> _table = null!;

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

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchIsPaid;
    private string? SearchIsPaid
    {
        get => _searchIsPaid;
        set
        {
            _searchIsPaid = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with loan schedule-specific configuration.
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

        Context = new EntityServerTableContext<LoanScheduleResponse, DefaultIdType, LoanScheduleViewModel>(
            fields:
            [
                new EntityField<LoanScheduleResponse>(dto => dto.InstallmentNumber, "Installment #", "InstallmentNumber", typeof(int)),
                new EntityField<LoanScheduleResponse>(dto => dto.DueDate, "Due Date", "DueDate", typeof(DateTimeOffset)),
                new EntityField<LoanScheduleResponse>(dto => dto.PrincipalAmount, "Principal", "PrincipalAmount", typeof(decimal)),
                new EntityField<LoanScheduleResponse>(dto => dto.InterestAmount, "Interest", "InterestAmount", typeof(decimal)),
                new EntityField<LoanScheduleResponse>(dto => dto.TotalAmount, "Total", "TotalAmount", typeof(decimal)),
                new EntityField<LoanScheduleResponse>(dto => dto.PaidAmount, "Paid", "PaidAmount", typeof(decimal)),
                new EntityField<LoanScheduleResponse>(dto => dto.IsPaid, "Is Paid", "IsPaid", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoanSchedulesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLoanSchedulesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanScheduleResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            entityName: "Loan Schedule",
            entityNamePlural: "Loan Schedules",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => true);
    }

    /// <summary>
    /// Show help dialog.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<LoanSchedulesHelpDialog>("Loan Schedules Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View schedule details in a dialog.
    /// </summary>
    private async Task ViewScheduleDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanScheduleDetailsDialog.ScheduleId), id }
        };

        await DialogService.ShowAsync<LoanScheduleDetailsDialog>("Schedule Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
