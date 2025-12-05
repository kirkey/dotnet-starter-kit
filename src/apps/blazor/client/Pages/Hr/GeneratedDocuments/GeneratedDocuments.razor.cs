using FSH.Starter.Blazor.Infrastructure.Api;

namespace FSH.Starter.Blazor.Client.Pages.Hr.GeneratedDocuments;

public partial class GeneratedDocuments
{
    protected EntityServerTableContext<GeneratedDocumentResponse, DefaultIdType, GeneratedDocumentViewModel> Context { get; set; } = null!;

    private EntityTable<GeneratedDocumentResponse, DefaultIdType, GeneratedDocumentViewModel>? _table;

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

        Context = new EntityServerTableContext<GeneratedDocumentResponse, DefaultIdType, GeneratedDocumentViewModel>(
            entityName: "Generated Document",
            entityNamePlural: "Generated Documents",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<GeneratedDocumentResponse>(r => r.EntityType ?? "-", "Entity Type", "EntityType"),
                new EntityField<GeneratedDocumentResponse>(r => r.Status ?? "-", "Status", "Status"),
                new EntityField<GeneratedDocumentResponse>(r => r.GeneratedDate.ToShortDateString(), "Generated", "GeneratedDate"),
                new EntityField<GeneratedDocumentResponse>(r => r.FinalizedDate?.ToShortDateString() ?? "-", "Finalized", "FinalizedDate"),
                new EntityField<GeneratedDocumentResponse>(r => r.SignedBy ?? "-", "Signed By", "SignedBy"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchGeneratedDocumentsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchGeneratedDocumentsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<GeneratedDocumentResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreateGeneratedDocumentCommand
                {
                    DocumentTemplateId = vm.DocumentTemplateId,
                    EntityId = vm.EntityId,
                    EntityType = vm.EntityType,
                    GeneratedContent = vm.GeneratedContent,
                    Notes = vm.Notes
                };
                await Client.CreateGeneratedDocumentEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateGeneratedDocumentCommand
                {
                    Id = id,
                    Status = vm.Status,
                    FilePath = vm.FilePath,
                    SignedBy = vm.SignedBy,
                    Notes = vm.Notes
                };
                await Client.UpdateGeneratedDocumentEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteGeneratedDocumentEndpointAsync("1", id).ConfigureAwait(false),
            hasExtraActionsFunc: () => true);

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<GeneratedDocumentsHelpDialog>("Generated Documents Help", new DialogParameters(), _dialogOptions);
    }

    private async Task ViewDocumentAsync(GeneratedDocumentResponse doc)
    {
        var parameters = new DialogParameters
        {
            { nameof(GeneratedDocumentViewDialog.Document), doc }
        };
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };
        await DialogService.ShowAsync<GeneratedDocumentViewDialog>("View Document", parameters, options);
    }

    private async Task FinalizeDocumentAsync(GeneratedDocumentResponse doc)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Finalize Document",
            "Are you sure you want to finalize this document? This action cannot be undone.",
            yesText: "Finalize", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                var command = new UpdateGeneratedDocumentCommand
                {
                    Id = doc.Id,
                    Status = "Finalized"
                };
                await Client.UpdateGeneratedDocumentEndpointAsync("1", doc.Id, command).ConfigureAwait(false);
                Snackbar.Add("Document finalized successfully", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error finalizing document: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task SignDocumentAsync(GeneratedDocumentResponse doc)
    {
        // In a real implementation, this would open a signing dialog
        Snackbar.Add("Document signing initiated", Severity.Info);
        await Task.CompletedTask;
    }

    private async Task DownloadDocumentAsync(GeneratedDocumentResponse doc)
    {
        if (!string.IsNullOrEmpty(doc.FilePath))
        {
            Snackbar.Add("Downloading document...", Severity.Info);
            await Task.CompletedTask;
        }
    }
}

public class GeneratedDocumentViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType DocumentTemplateId { get; set; }
    public DefaultIdType EntityId { get; set; }
    public string? EntityType { get; set; } = "Employee";
    public string? GeneratedContent { get; set; }
    public string? Status { get; set; } = "Draft";
    public string? FilePath { get; set; }
    public string? SignedBy { get; set; }
    public string? Notes { get; set; }
}
