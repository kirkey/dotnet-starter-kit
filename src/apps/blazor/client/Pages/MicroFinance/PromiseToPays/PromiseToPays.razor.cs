namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.PromiseToPays;

public partial class PromiseToPays
{
    protected EntityServerTableContext<PromiseToPaySummaryResponse, DefaultIdType, PromiseToPayViewModel> Context { get; set; } = null!;
    private EntityTable<PromiseToPaySummaryResponse, DefaultIdType, PromiseToPayViewModel> _table = null!;

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

        Context = new EntityServerTableContext<PromiseToPaySummaryResponse, DefaultIdType, PromiseToPayViewModel>(
            fields:
            [
                new EntityField<PromiseToPaySummaryResponse>(dto => dto.PromiseDate, "Promise Date", "PromiseDate", typeof(DateTimeOffset)),
                new EntityField<PromiseToPaySummaryResponse>(dto => dto.PromisedPaymentDate, "Payment Date", "PromisedPaymentDate", typeof(DateTimeOffset)),
                new EntityField<PromiseToPaySummaryResponse>(dto => dto.PromisedAmount, "Promised", "PromisedAmount", typeof(decimal)),
                new EntityField<PromiseToPaySummaryResponse>(dto => dto.ActualAmountPaid, "Paid", "ActualAmountPaid", typeof(decimal)),
                new EntityField<PromiseToPaySummaryResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<PromiseToPaySummaryResponse>(dto => dto.RescheduleCount, "Reschedules", "RescheduleCount", typeof(int)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchPromiseToPaysCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchPromiseToPaysAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PromiseToPaySummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreatePromiseToPayAsync("1", viewModel.Adapt<CreatePromiseToPayCommand>()).ConfigureAwait(false);
            },
            entityName: "Promise To Pay",
            entityNamePlural: "Promise To Pays",
            entityResource: FshResources.PromiseToPays,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var promise = await Client.GetPromiseToPayAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Promise", promise } };
        await DialogService.ShowAsync<PromiseToPayDetailsDialog>("Promise To Pay Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show promise to pay help dialog.
    /// </summary>
    private async Task ShowPromiseToPayHelp()
    {
        await DialogService.ShowAsync<PromiseToPaysHelpDialog>("Promise To Pay Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
