using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using Shared.Authorization;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.Accounts;

public partial class Accounts
{
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;

    protected EntityServerTableContext<AccountResponse, DefaultIdType, AccountViewModel> Context { get; set; } = default!;

    private EntityTable<AccountResponse, DefaultIdType, AccountViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<AccountResponse, DefaultIdType, AccountViewModel>(
            entityName: "Account",
            entityNamePlural: "Accounts",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<AccountResponse>(response => response.AccountCategory, "AccountCategory", "AccountCategory"),
                new EntityField<AccountResponse>(response => response.Type, "Type", "Type"),
                new EntityField<AccountResponse>(response => response.ParentCode, "Parent", "ParentCode"),
                new EntityField<AccountResponse>(response => response.Code, "Code", "Code"),
                new EntityField<AccountResponse>(response => response.Name, "Name", "Name"),
                new EntityField<AccountResponse>(response => response.Balance, "Balance", "Balance")
            ],
            enableAdvancedSearch: false,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<AccountSearchRequest>();

                var result = await ApiClient.AccountSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<AccountResponse>>();
            },
            createFunc: async account =>
            {
                await ApiClient.AccountCreateEndpointAsync("1", account.Adapt<AccountCreateRequest>());
            },
            updateFunc: async (id, account) =>
            {
                await ApiClient.AccountUpdateEndpointAsync("1", id, account.Adapt<AccountUpdateRequest>());
            },
            deleteFunc: async id => await ApiClient.AccountDeleteEndpointAsync("1", id));
}

public class AccountViewModel : AccountUpdateRequest
{
}
