using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanGuarantors.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanGuarantors;

public partial class LoanGuarantors
{
    [Inject]
    private IAuthorizationService AuthorizationService { get; set; } = null!;

    private ClientPreference _clientPreference = new();
    private EntityTable<LoanGuarantorResponse, DefaultIdType, LoanGuarantorViewModel>? _table;
    private EntityServerTableContext<LoanGuarantorResponse, DefaultIdType, LoanGuarantorViewModel> _context = null!;

    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    private bool _canApprove;
    private bool _canReject;

    private string? _selectedStatus;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Create, FshResources.LoanGuarantors))).Succeeded;
        _canUpdate = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Update, FshResources.LoanGuarantors))).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Delete, FshResources.LoanGuarantors))).Succeeded;
        _canApprove = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Approve, FshResources.LoanGuarantors))).Succeeded;
        _canReject = (await AuthorizationService.AuthorizeAsync(state.User, FshPermission.NameFor(FshActions.Reject, FshResources.LoanGuarantors))).Succeeded;

        _context = new EntityServerTableContext<LoanGuarantorResponse, DefaultIdType, LoanGuarantorViewModel>(
            entityName: "Loan Guarantor",
            entityNamePlural: "Loan Guarantors",
            entityResource: FshResources.LoanGuarantors,
            fields: new List<EntityField<LoanGuarantorResponse>>
            {
                new EntityField<LoanGuarantorResponse>(g => g.GuarantorName, "Guarantor", "GuarantorName"),
                new EntityField<LoanGuarantorResponse>(g => g.MemberNumber, "Member #", "MemberNumber"),
                new EntityField<LoanGuarantorResponse>(g => g.LoanNumber, "Loan #", "LoanNumber"),
                new EntityField<LoanGuarantorResponse>(g => g.GuaranteedAmount, "Amount", "GuaranteedAmount", typeof(decimal)),
                new EntityField<LoanGuarantorResponse>(g => g.Relationship, "Relationship", "Relationship"),
                new EntityField<LoanGuarantorResponse>(g => g.Status, "Status", "Status"),
                new EntityField<LoanGuarantorResponse>(g => g.ExpiryDate, "Expiry", "ExpiryDate", typeof(DateOnly))
            },
            idFunc: g => g.Id,
            searchFunc: async filter =>
            {
                var request = filter.Adapt<SearchLoanGuarantorsCommand>();
                var response = await Client.SearchLoanGuarantorsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<LoanGuarantorResponse>>();
            },
            getDetailsFunc: async id => (await Client.GetLoanGuarantorEndpointAsync("1", id)).Adapt<LoanGuarantorViewModel>(),
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateLoanGuarantorCommand>();
                await Client.CreateLoanGuarantorEndpointAsync("1", command);
            },
            updateFunc: async (id, vm) =>
            {
                var command = vm.Adapt<UpdateLoanGuarantorCommand>();
                await Client.UpdateLoanGuarantorEndpointAsync("1", id, command);
            },
            deleteFunc: async id => await Client.DeleteLoanGuarantorEndpointAsync("1", id),
            hasExtraActionsFunc: () => true,
            canUpdateEntityFunc: _ => _canUpdate,
            canDeleteEntityFunc: _ => _canDelete);
    }

    private Color GetStatusColor(string? status) => status switch
    {
        "Pending" => Color.Warning,
        "Approved" => Color.Success,
        "Rejected" => Color.Error,
        "Expired" => Color.Default,
        _ => Color.Default
    };

    private async Task ShowDetails(LoanGuarantorResponse guarantor)
    {
        var parameters = new DialogParameters<LoanGuarantorDetailsDialog>
        {
            { x => x.Guarantor, guarantor }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<LoanGuarantorDetailsDialog>("Guarantor Details", parameters, options);
    }

    private async Task ApproveGuarantor(LoanGuarantorResponse guarantor)
    {
        var parameters = new DialogParameters<LoanGuarantorApproveDialog>
        {
            { x => x.GuarantorId, guarantor.Id },
            { x => x.GuarantorName, guarantor.GuarantorName ?? "Guarantor" },
            { x => x.GuaranteedAmount, guarantor.GuaranteedAmount }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanGuarantorApproveDialog>("Approve Guarantor", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task RejectGuarantor(LoanGuarantorResponse guarantor)
    {
        var parameters = new DialogParameters<LoanGuarantorRejectDialog>
        {
            { x => x.GuarantorId, guarantor.Id },
            { x => x.GuarantorName, guarantor.GuarantorName ?? "Guarantor" }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanGuarantorRejectDialog>("Reject Guarantor", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task UpdateAmount(LoanGuarantorResponse guarantor)
    {
        var parameters = new DialogParameters<LoanGuarantorUpdateAmountDialog>
        {
            { x => x.GuarantorId, guarantor.Id },
            { x => x.GuarantorName, guarantor.GuarantorName ?? "Guarantor" },
            { x => x.CurrentAmount, guarantor.GuaranteedAmount }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanGuarantorUpdateAmountDialog>("Update Guaranteed Amount", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ShowHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<LoanGuarantorsHelpDialog>("Loan Guarantors Help", options);
    }
}
