namespace FSH.Starter.Blazor.Client.Pages.Hr.DocumentTemplates;

public partial class DocumentTemplates
{
    protected EntityServerTableContext<DocumentTemplateResponse, DefaultIdType, DocumentTemplateViewModel> Context { get; set; } = null!;

    private EntityTable<DocumentTemplateResponse, DefaultIdType, DocumentTemplateViewModel>? _table;

    private ClientPreference _preference = new();

    private readonly DialogOptions _dialogOptions = new() 
    { 
        CloseOnEscapeKey = true, 
        MaxWidth = MaxWidth.Medium, 
        FullWidth = true 
    };

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

        Context = new EntityServerTableContext<DocumentTemplateResponse, DefaultIdType, DocumentTemplateViewModel>(
            entityName: "Document Template",
            entityNamePlural: "Document Templates",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<DocumentTemplateResponse>(r => r.TemplateName ?? "-", "Name", "TemplateName"),
                new EntityField<DocumentTemplateResponse>(r => r.DocumentType ?? "-", "Type", "DocumentType"),
                new EntityField<DocumentTemplateResponse>(r => $"v{r.Version}", "Version", "Version"),
                new EntityField<DocumentTemplateResponse>(r => r.IsActive ? "Active" : "Inactive", "Status", "IsActive"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchDocumentTemplatesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchDocumentTemplatesEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<DocumentTemplateResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreateDocumentTemplateCommand
                {
                    TemplateName = vm.TemplateName,
                    DocumentType = vm.DocumentType,
                    TemplateContent = vm.TemplateContent,
                    TemplateVariables = vm.TemplateVariables,
                    Description = vm.Description
                };
                await Client.CreateDocumentTemplateEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateDocumentTemplateCommand
                {
                    Id = id,
                    TemplateName = vm.TemplateName,
                    TemplateContent = vm.TemplateContent,
                    TemplateVariables = vm.TemplateVariables,
                    Description = vm.Description
                };
                await Client.UpdateDocumentTemplateEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteDocumentTemplateEndpointAsync("1", id).ConfigureAwait(false),
            hasExtraActionsFunc: () => true);

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<DocumentTemplatesHelpDialog>("Document Templates Help", new DialogParameters(), _dialogOptions);
    }

    private async Task PreviewTemplateAsync(DocumentTemplateResponse template)
    {
        var parameters = new DialogParameters
        {
            { nameof(DocumentTemplatePreviewDialog.Template), template }
        };
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };
        await DialogService.ShowAsync<DocumentTemplatePreviewDialog>("Template Preview", parameters, options);
    }

    private async Task DuplicateTemplateAsync(DocumentTemplateResponse template)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Duplicate Template",
            $"Create a copy of '{template.TemplateName}'?",
            yesText: "Duplicate", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                var command = new CreateDocumentTemplateCommand
                {
                    TemplateName = $"{template.TemplateName} (Copy)",
                    DocumentType = template.DocumentType,
                    TemplateContent = template.TemplateContent,
                    TemplateVariables = template.TemplateVariables,
                    Description = template.Description
                };
                await Client.CreateDocumentTemplateEndpointAsync("1", command).ConfigureAwait(false);
                Snackbar.Add("Template duplicated successfully", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error duplicating template: {ex.Message}", Severity.Error);
            }
        }
    }
}

public class DocumentTemplateViewModel
{
    public DefaultIdType Id { get; set; }
    public string? TemplateName { get; set; }
    public string? DocumentType { get; set; } = "EmploymentContract";
    public string? TemplateContent { get; set; }
    public string? TemplateVariables { get; set; }
    public string? Description { get; set; }
}
