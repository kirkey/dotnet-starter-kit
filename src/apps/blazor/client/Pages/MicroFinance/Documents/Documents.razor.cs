namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Documents;

public partial class Documents
{
    protected EntityServerTableContext<DocumentSummaryResponse, DefaultIdType, DocumentViewModel> Context { get; set; } = null!;
    private EntityTable<DocumentSummaryResponse, DefaultIdType, DocumentViewModel> _table = null!;

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

        Context = new EntityServerTableContext<DocumentSummaryResponse, DefaultIdType, DocumentViewModel>(
            fields:
            [
                new EntityField<DocumentSummaryResponse>(dto => dto.OriginalFileName, "File Name", "OriginalFileName"),
                new EntityField<DocumentSummaryResponse>(dto => dto.DocumentType, "Type", "DocumentType"),
                new EntityField<DocumentSummaryResponse>(dto => dto.Category, "Category", "Category"),
                new EntityField<DocumentSummaryResponse>(dto => dto.EntityType, "Entity Type", "EntityType"),
                new EntityField<DocumentSummaryResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<DocumentSummaryResponse>(dto => dto.IsVerified, "Verified", "IsVerified", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchDocumentsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchDocumentsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<DocumentSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateDocumentAsync("1", viewModel.Adapt<CreateDocumentCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                var command = viewModel.Adapt<UpdateDocumentCommand>();
                command.Id = id;
                await Client.UpdateDocumentAsync("1", id, command).ConfigureAwait(false);
            },
            entityName: "Document",
            entityNamePlural: "Documents",
            entityResource: FshResources.Documents,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var document = await Client.GetDocumentAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Document", document } };
        await DialogService.ShowAsync<DocumentDetailsDialog>("Document Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show document help dialog.
    /// </summary>
    private async Task ShowDocumentHelp()
    {
        await DialogService.ShowAsync<DocumentsHelpDialog>("Document Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
