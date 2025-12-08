namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ReportDefinitions;

public partial class ReportDefinitions
{
    protected EntityServerTableContext<ReportDefinitionResponse, DefaultIdType, ReportDefinitionViewModel> Context { get; set; } = null!;
    private EntityTable<ReportDefinitionResponse, DefaultIdType, ReportDefinitionViewModel> _table = null!;

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

        Context = new EntityServerTableContext<ReportDefinitionResponse, DefaultIdType, ReportDefinitionViewModel>(
            fields:
            [
                new EntityField<ReportDefinitionResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<ReportDefinitionResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<ReportDefinitionResponse>(dto => dto.Category, "Category", "Category"),
                new EntityField<ReportDefinitionResponse>(dto => dto.OutputFormat, "Format", "OutputFormat"),
                new EntityField<ReportDefinitionResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<ReportDefinitionResponse>(dto => dto.IsScheduled, "Scheduled", "IsScheduled", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchReportDefinitionsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchReportDefinitionsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ReportDefinitionResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async vm =>
            {
                var command = new CreateReportDefinitionCommand
                {
                    Code = vm.Code,
                    Name = vm.Name,
                    Category = vm.Category,
                    OutputFormat = vm.OutputFormat,
                    Description = vm.Description,
                    ParametersDefinition = vm.ParametersDefinition,
                    Query = vm.Query,
                    LayoutTemplate = vm.LayoutTemplate
                };
                await Client.CreateReportDefinitionAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateReportDefinitionCommand
                {
                    Id = id,
                    Name = vm.Name,
                    OutputFormat = vm.OutputFormat,
                    Description = vm.Description,
                    ParametersDefinition = vm.ParametersDefinition,
                    Query = vm.Query,
                    LayoutTemplate = vm.LayoutTemplate
                };
                await Client.UpdateReportDefinitionAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteReportDefinitionAsync("1", id).ConfigureAwait(false),
            entityName: "Report Definition",
            entityNamePlural: "Report Definitions",
            entityResource: FshResources.ReportDefinitions,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var report = await Client.GetReportDefinitionAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "ReportDefinition", report } };
        await DialogService.ShowAsync<ReportDefinitionDetailsDialog>("Report Definition Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }
}
