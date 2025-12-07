using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.CustomerCases.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Shared;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CustomerCases;

public partial class CustomerCases
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

    private EntityServerTableContext<CustomerCaseResponse, DefaultIdType, CustomerCaseViewModel> _context = null!;
    private EntityTable<CustomerCaseResponse, DefaultIdType, CustomerCaseViewModel>? _table;

    private bool _canCreate;
    private bool _canAssign;
    private bool _canEscalate;
    private bool _canResolve;
    private bool _canClose;
    private int _elevation;
    private string _borderRadius = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Create)).Succeeded;
        _canAssign = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Update)).Succeeded;
        _canEscalate = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Update)).Succeeded;
        _canResolve = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Update)).Succeeded;
        _canClose = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Close)).Succeeded;

        _elevation = ClientPreference.Elevation;
        _borderRadius = $"border-radius: {ClientPreference.BorderRadius}px";

        _context = new EntityServerTableContext<CustomerCaseResponse, DefaultIdType, CustomerCaseViewModel>(
            entityName: "Customer Case",
            entityNamePlural: "Customer Cases",
            entityResource: FshResources.MicroFinance,
            searchAction: FshActions.Search,
            fields:
            [
                new EntityField<CustomerCaseResponse>(c => c.CaseNumber!, "Case #", "CaseNumber"),
                new EntityField<CustomerCaseResponse>(c => c.Subject!, "Subject", "Subject"),
                new EntityField<CustomerCaseResponse>(c => c.Category!, "Category", "Category"),
                new EntityField<CustomerCaseResponse>(c => c.Priority!, "Priority", "Priority"),
                new EntityField<CustomerCaseResponse>(c => c.Channel!, "Channel", "Channel"),
                new EntityField<CustomerCaseResponse>(c => c.Status!, "Status", "Status"),
                new EntityField<CustomerCaseResponse>(c => c.AssignedToName!, "Assigned To", "AssignedToName"),
            ],
            enableAdvancedSearch: false,
            idFunc: c => c.Id,
            searchFunc: async filter =>
            {
                var response = await MicroFinanceClient.SearchCustomerCasesEndpointAsync("1", new SearchCustomerCasesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy ?? []
                });

                return response.Adapt<PaginationResponse<CustomerCaseResponse>>();
            },
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateCustomerCaseCommand>();
                await MicroFinanceClient.CreateCustomerCaseAsync("1", command);
            },
            getDetailsFunc: async id =>
            {
                var response = await MicroFinanceClient.GetCustomerCaseEndpointAsync("1", id);
                return response.Adapt<CustomerCaseViewModel>();
            },
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(CustomerCaseResponse customerCase)
    {
        var parameters = new DialogParameters
        {
            { nameof(CustomerCaseDetailsDialog.CustomerCase), customerCase }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<CustomerCaseDetailsDialog>("Customer Case Details", parameters, options);
    }

    private async Task AssignCase(CustomerCaseResponse customerCase)
    {
        var parameters = new DialogParameters
        {
            { nameof(CustomerCaseAssignDialog.CaseId), customerCase.Id },
            { nameof(CustomerCaseAssignDialog.CaseNumber), customerCase.CaseNumber }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CustomerCaseAssignDialog>("Assign Case", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task EscalateCase(CustomerCaseResponse customerCase)
    {
        var parameters = new DialogParameters
        {
            { nameof(CustomerCaseEscalateDialog.CaseId), customerCase.Id },
            { nameof(CustomerCaseEscalateDialog.CaseNumber), customerCase.CaseNumber }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CustomerCaseEscalateDialog>("Escalate Case", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ResolveCase(CustomerCaseResponse customerCase)
    {
        var parameters = new DialogParameters
        {
            { nameof(CustomerCaseResolveDialog.CaseId), customerCase.Id },
            { nameof(CustomerCaseResolveDialog.CaseNumber), customerCase.CaseNumber }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CustomerCaseResolveDialog>("Resolve Case", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task CloseCase(CustomerCaseResponse customerCase)
    {
        var parameters = new DialogParameters
        {
            { nameof(CustomerCaseCloseDialog.CaseId), customerCase.Id },
            { nameof(CustomerCaseCloseDialog.CaseNumber), customerCase.CaseNumber }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CustomerCaseCloseDialog>("Close Case", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ShowHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<CustomerCasesHelpDialog>("Customer Cases Help", options);
    }
}
