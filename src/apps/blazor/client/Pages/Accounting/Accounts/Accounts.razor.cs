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
                new EntityField<AccountResponse>(response => response.Category, "Category", "Category"),
                new EntityField<AccountResponse>(response => response.TransactionType, "TransactionType", "TransactionType"),
                new EntityField<AccountResponse>(response => response.ParentCode, "Parent", "ParentCode"),
                new EntityField<AccountResponse>(response => response.Code, "Code", "Code"),
                new EntityField<AccountResponse>(response => response.Name, "Name", "Name"),
                new EntityField<AccountResponse>(response => response.Balance, "Balance", "Balance")
            ],
            enableAdvancedSearch: false,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchAccountsCommand>();

                var result = await ApiClient.SearchAccountsEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<AccountResponse>>();
            },
            createFunc: async account =>
            {
                await ApiClient.CreateAccountEndpointAsync("1", account.Adapt<CreateAccountCommand>());
            },
            updateFunc: async (id, account) =>
            {
                await ApiClient.UpdateAccountEndpointAsync("1", id, account.Adapt<UpdateAccountCommand>());
            },
            deleteFunc: async id => await ApiClient.DeleteAccountEndpointAsync("1", id));
}

public class AccountViewModel : UpdateAccountCommand
{
}
