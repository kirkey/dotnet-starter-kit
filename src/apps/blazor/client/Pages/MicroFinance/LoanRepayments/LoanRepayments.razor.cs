using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanRepayments.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Shared;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanRepayments;

public partial class LoanRepayments
{
    [Inject]
    protected IAuthorizationService AuthorizationService { get; set; } = null!;

    [Inject]
    protected ClientPreference ClientPreference { get; set; } = null!;

    private EntityServerTableContext<LoanRepaymentResponse, DefaultIdType, LoanRepaymentViewModel> _context = null!;
    private EntityTable<LoanRepaymentResponse, DefaultIdType, LoanRepaymentViewModel>? _table;

    private bool _canCreate;
    private bool _canView;
    private bool _canReverse;
    private bool _canPrint;
    private int _elevation;
    private string _borderRadius = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Create)).Succeeded;
        _canView = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.View)).Succeeded;
        _canReverse = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.Update)).Succeeded;
        _canPrint = (await AuthorizationService.AuthorizeAsync(state.User, FshPermissions.MicroFinance.View)).Succeeded;

        _elevation = ClientPreference.Elevation;
        _borderRadius = $"border-radius: {ClientPreference.BorderRadius}px";

        _context = new EntityServerTableContext<LoanRepaymentResponse, DefaultIdType, LoanRepaymentViewModel>(
            entityName: "Loan Repayment",
            entityNamePlural: "Loan Repayments",
            entityResource: FshResources.MicroFinance,
            searchAction: FshActions.Search,
            fields:
            [
                new EntityField<LoanRepaymentResponse>(r => r.LoanNumber!, "Loan #", "LoanNumber"),
                new EntityField<LoanRepaymentResponse>(r => r.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<LoanRepaymentResponse>(r => r.PrincipalAmount, "Principal", "PrincipalAmount", typeof(decimal)),
                new EntityField<LoanRepaymentResponse>(r => r.InterestAmount, "Interest", "InterestAmount", typeof(decimal)),
                new EntityField<LoanRepaymentResponse>(r => r.PaymentMethod!, "Method", "PaymentMethod"),
                new EntityField<LoanRepaymentResponse>(r => r.PaymentDate, "Date", "PaymentDate"),
                new EntityField<LoanRepaymentResponse>(r => r.ReceiptNumber!, "Receipt #", "ReceiptNumber"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var response = await Client.SearchLoanRepaymentsEndpointAsync("1", new SearchLoanRepaymentsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy ?? []
                });

                return response.Adapt<PaginationResponse<LoanRepaymentResponse>>();
            },
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateLoanRepaymentCommand>();
                await Client.CreateLoanRepaymentAsync("1", command);
            },
            getDetailsFunc: async id =>
            {
                var response = await Client.GetLoanRepaymentEndpointAsync("1", id);
                return response.Adapt<LoanRepaymentViewModel>();
            },
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(LoanRepaymentResponse repayment)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanRepaymentDetailsDialog.Repayment), repayment }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<LoanRepaymentDetailsDialog>("Loan Repayment Details", parameters, options);
    }

    private async Task ReverseRepayment(LoanRepaymentResponse repayment)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanRepaymentReverseDialog.RepaymentId), repayment.Id },
            { nameof(LoanRepaymentReverseDialog.ReceiptNumber), repayment.ReceiptNumber },
            { nameof(LoanRepaymentReverseDialog.Amount), repayment.Amount }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanRepaymentReverseDialog>("Reverse Repayment", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task PrintReceipt(LoanRepaymentResponse repayment)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanRepaymentReceiptDialog.Repayment), repayment }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<LoanRepaymentReceiptDialog>("Payment Receipt", parameters, options);
    }

    private async Task ShowHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<LoanRepaymentsHelpDialog>("Loan Repayments Help", options);
    }
}
