using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.Documents.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Shared;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Documents;

public partial class Documents
{
    [Inject]
    protected IMicroFinanceClient MicroFinanceClient { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthorizationService { get; set; } = null!;

    [Inject]
    protected IDialogService DialogService { get; set; } = null!;

    [Inject]
    protected ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    protected ClientPreference ClientPreference { get; set; } = null!;

    private EntityServerTableContext<DocumentResponse, DefaultIdType, DocumentViewModel> _context = null!;
    private EntityTable<DocumentResponse, DefaultIdType, DocumentViewModel>? _table;

    private bool _canCreate;
    private bool _canVerify;
    private bool _canDownload;
    private int _elevation;
    private string _borderRadius = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Create)).Succeeded;
        _canVerify = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Update)).Succeeded;
        _canDownload = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.View)).Succeeded;

        _elevation = ClientPreference.Elevation;
        _borderRadius = $"border-radius: {ClientPreference.BorderRadius}px";

        _context = new EntityServerTableContext<DocumentResponse, DefaultIdType, DocumentViewModel>(
            entityName: "Document",
            entityNamePlural: "Documents",
            entityResource: FshResources.MicroFinance,
            searchAction: FshActions.Search,
            fields:
            [
                new EntityField<DocumentResponse>(d => d.Name!, "Name", "Name"),
                new EntityField<DocumentResponse>(d => d.DocumentType!, "Type", "DocumentType"),
                new EntityField<DocumentResponse>(d => d.Category!, "Category", "Category"),
                new EntityField<DocumentResponse>(d => d.EntityType!, "Entity Type", "EntityType"),
                new EntityField<DocumentResponse>(d => FormatFileSize(d.FileSizeBytes), "Size", "FileSizeBytes"),
                new EntityField<DocumentResponse>(d => d.IsVerified ? "Verified" : "Pending", "Status", "IsVerified"),
            ],
            enableAdvancedSearch: false,
            idFunc: d => d.Id,
            searchFunc: async filter =>
            {
                var response = await MicroFinanceClient.SearchDocumentsEndpointAsync("1", new SearchDocumentsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy ?? []
                });

                return response.Adapt<PaginationResponse<DocumentResponse>>();
            },
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateDocumentCommand>();
                await MicroFinanceClient.CreateDocumentAsync("1", command);
            },
            getDetailsFunc: async id =>
            {
                var response = await MicroFinanceClient.GetDocumentEndpointAsync("1", id);
                return response.Adapt<DocumentViewModel>();
            },
            hasExtraActionsFunc: () => true);
    }

    private static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }

    private async Task ViewDetails(DocumentResponse document)
    {
        var parameters = new DialogParameters
        {
            { nameof(DocumentDetailsDialog.Document), document }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<DocumentDetailsDialog>("Document Details", parameters, options);
    }

    private async Task VerifyDocument(DocumentResponse document)
    {
        var parameters = new DialogParameters
        {
            { nameof(DocumentVerifyDialog.DocumentId), document.Id },
            { nameof(DocumentVerifyDialog.DocumentName), document.Name }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<DocumentVerifyDialog>("Verify Document", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private void DownloadDocument(DocumentResponse document)
    {
        Snackbar.Add($"Download functionality for '{document.Name}' would be implemented here.", Severity.Info);
    }

    private async Task ShowHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<DocumentsHelpDialog>("Documents Help", options);
    }
}
