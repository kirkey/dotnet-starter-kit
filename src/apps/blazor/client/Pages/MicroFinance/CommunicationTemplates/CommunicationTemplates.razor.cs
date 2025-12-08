namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CommunicationTemplates;

public partial class CommunicationTemplates
{
    protected EntityServerTableContext<CommunicationTemplateSummaryResponse, DefaultIdType, CommunicationTemplateViewModel> Context { get; set; } = null!;
    private EntityTable<CommunicationTemplateSummaryResponse, DefaultIdType, CommunicationTemplateViewModel> _table = null!;

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

        Context = new EntityServerTableContext<CommunicationTemplateSummaryResponse, DefaultIdType, CommunicationTemplateViewModel>(
            fields:
            [
                new EntityField<CommunicationTemplateSummaryResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<CommunicationTemplateSummaryResponse>(dto => dto.Channel, "Channel", "Channel"),
                new EntityField<CommunicationTemplateSummaryResponse>(dto => dto.Category, "Category", "Category"),
                new EntityField<CommunicationTemplateSummaryResponse>(dto => dto.Subject, "Subject", "Subject"),
                new EntityField<CommunicationTemplateSummaryResponse>(dto => dto.Language, "Language", "Language"),
                new EntityField<CommunicationTemplateSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCommunicationTemplatesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCommunicationTemplatesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CommunicationTemplateSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCommunicationTemplateAsync("1", viewModel.Adapt<CreateCommunicationTemplateCommand>()).ConfigureAwait(false);
            },
            entityName: "Communication Template",
            entityNamePlural: "Communication Templates",
            entityResource: FshResources.CommunicationTemplates,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var template = await Client.GetCommunicationTemplateAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Template", template } };
        await DialogService.ShowAsync<CommunicationTemplateDetailsDialog>("Communication Template Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }
}
