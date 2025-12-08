namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MarketingCampaigns;

public partial class MarketingCampaigns
{
    protected EntityServerTableContext<MarketingCampaignSummaryResponse, DefaultIdType, MarketingCampaignViewModel> Context { get; set; } = null!;
    private EntityTable<MarketingCampaignSummaryResponse, DefaultIdType, MarketingCampaignViewModel> _table = null!;

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

        Context = new EntityServerTableContext<MarketingCampaignSummaryResponse, DefaultIdType, MarketingCampaignViewModel>(
            fields:
            [
                new EntityField<MarketingCampaignSummaryResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<MarketingCampaignSummaryResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<MarketingCampaignSummaryResponse>(dto => dto.CampaignType, "Type", "CampaignType"),
                new EntityField<MarketingCampaignSummaryResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<MarketingCampaignSummaryResponse>(dto => dto.Budget, "Budget", "Budget", typeof(decimal)),
                new EntityField<MarketingCampaignSummaryResponse>(dto => dto.ConversionRate, "Conversion", "ConversionRate", typeof(decimal)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchMarketingCampaignsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchMarketingCampaignsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<MarketingCampaignSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateMarketingCampaignAsync("1", viewModel.Adapt<CreateMarketingCampaignCommand>()).ConfigureAwait(false);
            },
            entityName: "Marketing Campaign",
            entityNamePlural: "Marketing Campaigns",
            entityResource: FshResources.MarketingCampaigns,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var campaign = await Client.GetMarketingCampaignAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Campaign", campaign } };
        await DialogService.ShowAsync<MarketingCampaignDetailsDialog>("Marketing Campaign Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }
}
