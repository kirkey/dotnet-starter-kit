using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using Shared.Authorization;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.Accounts;

public partial class ChartOfAccounts
{
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;

    protected EntityServerTableContext<ChartOfAccountDto, DefaultIdType, ChartOfAccountViewModel> Context { get; set; } = default!;

    private EntityTable<ChartOfAccountDto, DefaultIdType, ChartOfAccountViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<ChartOfAccountDto, DefaultIdType, ChartOfAccountViewModel>(
            entityName: "Chart Of Account",
            entityNamePlural: "Chart Of Accounts",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<ChartOfAccountDto>(dto => dto.AccountCategory, "Category", "AccountCategory"),
                new EntityField<ChartOfAccountDto>(dto => dto.AccountType, "Type", "AccountType"),
                new EntityField<ChartOfAccountDto>(dto => dto.ParentCode, "Parent", "ParentCode"),
                new EntityField<ChartOfAccountDto>(dto => dto.Code, "Code", "Code"),
                new EntityField<ChartOfAccountDto>(dto => dto.Name, "Name", "Name"),
                new EntityField<ChartOfAccountDto>(dto => dto.Balance, "Balance", "Balance", typeof(decimal)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<ChartOfAccountSearchRequest>();

                var result = await ApiClient.AccountSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<ChartOfAccountDto>>();
            },
            createFunc: async account =>
            {
                await ApiClient.AccountCreateEndpointAsync("1", account.Adapt<ChartOfAccountCreateRequest>());
            },
            updateFunc: async (id, account) =>
            {
                await ApiClient.AccountUpdateEndpointAsync("1", id, account.Adapt<ChartOfAccountUpdateRequest>());
            },
            deleteFunc: async id => await ApiClient.AccountDeleteEndpointAsync("1", id));
}

public class ChartOfAccountViewModel : ChartOfAccountUpdateRequest
{
}
