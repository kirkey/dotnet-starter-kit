namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CreditBureauInquiries;

public partial class CreditBureauInquiries
{
    protected EntityServerTableContext<CreditBureauInquirySummaryResponse, DefaultIdType, CreditBureauInquiryViewModel> Context { get; set; } = null!;
    private EntityTable<CreditBureauInquirySummaryResponse, DefaultIdType, CreditBureauInquiryViewModel> _table = null!;

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

        Context = new EntityServerTableContext<CreditBureauInquirySummaryResponse, DefaultIdType, CreditBureauInquiryViewModel>(
            fields:
            [
                new EntityField<CreditBureauInquirySummaryResponse>(dto => dto.InquiryNumber, "Inquiry #", "InquiryNumber"),
                new EntityField<CreditBureauInquirySummaryResponse>(dto => dto.BureauName, "Bureau", "BureauName"),
                new EntityField<CreditBureauInquirySummaryResponse>(dto => dto.Purpose, "Purpose", "Purpose"),
                new EntityField<CreditBureauInquirySummaryResponse>(dto => dto.InquiryDate, "Inquiry Date", "InquiryDate", typeof(DateTime)),
                new EntityField<CreditBureauInquirySummaryResponse>(dto => dto.RequestedBy, "Requested By", "RequestedBy"),
                new EntityField<CreditBureauInquirySummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCreditBureauInquiriesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCreditBureauInquiriesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CreditBureauInquirySummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCreditBureauInquiryAsync("1", viewModel.Adapt<CreateCreditBureauInquiryCommand>()).ConfigureAwait(false);
            },
            entityName: "Credit Bureau Inquiry",
            entityNamePlural: "Credit Bureau Inquiries",
            entityResource: FshResources.CreditBureauInquiries,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var inquiry = await Client.GetCreditBureauInquiryAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Inquiry", inquiry } };
        await DialogService.ShowAsync<CreditBureauInquiryDetailsDialog>("Credit Bureau Inquiry Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show Credit Bureau Inquiries help dialog.
    /// </summary>
    private async Task ShowCreditBureauInquiriesHelp()
    {
        await DialogService.ShowAsync<CreditBureauInquiriesHelpDialog>("Credit Bureau Inquiries Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
