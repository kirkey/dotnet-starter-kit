namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanWriteOffs;

/// <summary>
/// Loan Write-Offs page logic. Manages loan write-off requests.
/// </summary>
public partial class LoanWriteOffs
{
    protected EntityServerTableContext<LoanWriteOffSummaryResponse, DefaultIdType, LoanWriteOffViewModel> Context { get; set; } = null!;
    private EntityTable<LoanWriteOffSummaryResponse, DefaultIdType, LoanWriteOffViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
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

        Context = new EntityServerTableContext<LoanWriteOffSummaryResponse, DefaultIdType, LoanWriteOffViewModel>(
            fields:
            [
                new EntityField<LoanWriteOffSummaryResponse>(dto => dto.WriteOffNumber, "Number", "WriteOffNumber"),
                new EntityField<LoanWriteOffSummaryResponse>(dto => dto.LoanId, "Loan", "LoanId"),
                new EntityField<LoanWriteOffSummaryResponse>(dto => dto.WriteOffType, "Type", "WriteOffType"),
                new EntityField<LoanWriteOffSummaryResponse>(dto => dto.TotalWriteOff, "Total Write-Off", "TotalWriteOff", typeof(decimal)),
                new EntityField<LoanWriteOffSummaryResponse>(dto => dto.RecoveredAmount, "Recovered", "RecoveredAmount", typeof(decimal)),
                new EntityField<LoanWriteOffSummaryResponse>(dto => dto.DaysPastDue, "Days Past Due", "DaysPastDue", typeof(int)),
                new EntityField<LoanWriteOffSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoanWriteOffsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLoanWriteOffsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanWriteOffSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateLoanWriteOffAsync("1", viewModel.Adapt<CreateLoanWriteOffCommand>()).ConfigureAwait(false);
            },
            entityName: "Loan Write-Off",
            entityNamePlural: "Loan Write-Offs",
            entityResource: FshResources.LoanWriteOffs,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var writeOff = await Client.GetLoanWriteOffAsync("1", id).ConfigureAwait(false);

        var parameters = new DialogParameters
        {
            { "WriteOff", writeOff }
        };

        await DialogService.ShowAsync<LoanWriteOffDetailsDialog>("Loan Write-Off Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show loan write-off help dialog.
    /// </summary>
    private async Task ShowLoanWriteOffHelp()
    {
        await DialogService.ShowAsync<LoanWriteOffHelpDialog>("Loan Write-Off Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
