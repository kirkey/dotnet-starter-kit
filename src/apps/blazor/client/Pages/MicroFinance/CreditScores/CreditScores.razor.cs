using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.CreditScores.Dialogs;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CreditScores;

public partial class CreditScores
{
    [Inject]
    private IAuthorizationService AuthorizationService { get; set; } = null!;

    private ClientPreference _clientPreference = new();
    private EntityTable<CreditScoreResponse, DefaultIdType, CreditScoreViewModel>? _table;
    private EntityServerTableContext<CreditScoreResponse, DefaultIdType, CreditScoreViewModel> _context = null!;

    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;

    private string? _selectedGrade;
    private string? _selectedSource;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState.GetAuthenticationStateAsync();
        _canCreate = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.CreditScores.Create)).Succeeded;
        _canUpdate = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.CreditScores.Update)).Succeeded;
        _canDelete = (await AuthorizationService.AuthorizeAsync(state.User, FSHPermissions.CreditScores.Delete)).Succeeded;

        _context = new EntityServerTableContext<CreditScoreResponse, DefaultIdType, CreditScoreViewModel>(
            entityName: "Credit Score",
            entityNamePlural: "Credit Scores",
            entityResource: FSHResources.CreditScores,
            fields: new List<EntityField<CreditScoreResponse>>
            {
                new EntityField<CreditScoreResponse>(s => s.MemberName, "Member", "MemberName"),
                new EntityField<CreditScoreResponse>(s => s.MemberNumber, "Member #", "MemberNumber"),
                new EntityField<CreditScoreResponse>(s => s.Score, "Score", "Score", typeof(decimal)),
                new EntityField<CreditScoreResponse>(s => s.Grade, "Grade", "Grade"),
                new EntityField<CreditScoreResponse>(s => s.Source, "Source", "Source"),
                new EntityField<CreditScoreResponse>(s => s.ProbabilityOfDefault, "PD %", "ProbabilityOfDefault", typeof(decimal)),
                new EntityField<CreditScoreResponse>(s => s.ValidUntil, "Valid Until", "ValidUntil", typeof(DateOnly))
            },
            idFunc: s => s.Id,
            searchFunc: async filter =>
            {
                var request = filter.Adapt<PaginationFilter>();
                var response = await Client.SearchCreditScoresEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<CreditScoreResponse>>();
            },
            getDetailsFunc: async id => (await Client.GetCreditScoreEndpointAsync("1", id)).Adapt<CreditScoreViewModel>(),
            createFunc: async vm =>
            {
                var command = vm.Adapt<CreateCreditScoreCommand>();
                await Client.CreateCreditScoreEndpointAsync("1", command);
            },
            updateFunc: async (id, vm) =>
            {
                var command = vm.Adapt<UpdateCreditScoreCommand>();
                await Client.UpdateCreditScoreEndpointAsync("1", id, command);
            },
            deleteFunc: async id => await Client.DeleteCreditScoreEndpointAsync("1", id),
            hasExtraActionsFunc: () => true,
            canUpdateEntityFunc: _ => _canUpdate,
            canDeleteEntityFunc: _ => _canDelete);
    }

    private Color GetScoreColor(decimal score, decimal maxScore)
    {
        var percentage = score / maxScore;
        return percentage switch
        {
            >= 0.8m => Color.Success,
            >= 0.6m => Color.Info,
            >= 0.4m => Color.Warning,
            _ => Color.Error
        };
    }

    private Color GetGradeColor(string? grade) => grade switch
    {
        "A" => Color.Success,
        "B" => Color.Info,
        "C" => Color.Warning,
        "D" => Color.Error,
        "E" => Color.Error,
        _ => Color.Default
    };

    private async Task ShowDetails(CreditScoreResponse score)
    {
        var parameters = new DialogParameters<CreditScoreDetailsDialog>
        {
            { x => x.CreditScore, score }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<CreditScoreDetailsDialog>("Credit Score Details", parameters, options);
    }

    private async Task RefreshScore(CreditScoreResponse score)
    {
        var parameters = new DialogParameters<CreditScoreRefreshDialog>
        {
            { x => x.MemberId, score.MemberId },
            { x => x.MemberName, score.MemberName ?? "Member" },
            { x => x.CurrentScore, score.Score }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CreditScoreRefreshDialog>("Refresh Credit Score", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await _table!.ReloadDataAsync();
        }
    }

    private async Task ShowScoreFactors(CreditScoreResponse score)
    {
        var parameters = new DialogParameters<CreditScoreFactorsDialog>
        {
            { x => x.CreditScore, score }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<CreditScoreFactorsDialog>("Score Factors", parameters, options);
    }

    private async Task ShowHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<CreditScoresHelpDialog>("Credit Scores Help", options);
    }
}
