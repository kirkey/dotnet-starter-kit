using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using Shared.Authorization;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.Accounts;

public partial class ChartOfAccounts : ComponentBase
{
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;

    protected EntityServerTableContext<ChartOfAccountDto, DefaultIdType, ChartOfAccountViewModel> Context { get; set; } = default!;

    private EntityTable<ChartOfAccountDto, DefaultIdType, ChartOfAccountViewModel> _table = default!;

    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<ChartOfAccountDto, DefaultIdType, ChartOfAccountViewModel>(
            entityName: "Chart Of Account",
            entityNamePlural: "Chart Of Accounts",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<ChartOfAccountDto>(dto => dto.UsoaCategory, "Category", "UsoaCategory"),
                new EntityField<ChartOfAccountDto>(dto => dto.AccountType, "Type", "AccountType"),
                new EntityField<ChartOfAccountDto>(dto => dto.ParentCode, "Parent", "ParentCode"),
                new EntityField<ChartOfAccountDto>(dto => dto.AccountCode, "Code", "AccountCode"),
                new EntityField<ChartOfAccountDto>(dto => dto.Name, "Name", "Name"),
                new EntityField<ChartOfAccountDto>(dto => dto.Balance, "Balance", "Balance", typeof(decimal)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<ChartOfAccountSearchRequest>();

                var result = await ApiClient.ChartOfAccountSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<ChartOfAccountDto>>();
            },
            createFunc: async account =>
            {
                await ApiClient.ChartOfAccountCreateEndpointAsync("1", account.Adapt<ChartOfAccountCreateRequest>());
            },
            updateFunc: async (id, account) =>
            {
                await ApiClient.ChartOfAccountUpdateEndpointAsync("1", id, account.Adapt<ChartOfAccountUpdateRequest>());
            },
            deleteFunc: async id => await ApiClient.ChartOfAccountDeleteEndpointAsync("1", id));
    }
}

public class ChartOfAccountViewModel : ChartOfAccountUpdateRequest
{
    // Properties will be inherited from ChartOfAccountUpdateRequest
    // This class serves as the view model for the entity table
}
