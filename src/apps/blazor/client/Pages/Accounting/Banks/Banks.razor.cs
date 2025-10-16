using FSH.Starter.Blazor.Client.Services;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.Banks;

/// <summary>
/// Banks page logic. Provides CRUD and search over Bank entities using the generated API client.
/// Follows the same patterns as other Accounting pages (Payees, ChartOfAccounts, Checks).
/// </summary>
public partial class Banks
{
    [Inject] protected ImageUrlService ImageUrlService { get; set; } = default!;

    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<BankResponse, DefaultIdType, BankViewModel> Context { get; set; } = default!;

    private EntityTable<BankResponse, DefaultIdType, BankViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<BankResponse, DefaultIdType, BankViewModel>(
            entityName: "Bank",
            entityNamePlural: "Banks",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<BankResponse>(response => response.ImageUrl, "Logo", "ImageUrl", Template: TemplateImage),
                new EntityField<BankResponse>(response => response.BankCode, "Bank Code", "BankCode"),
                new EntityField<BankResponse>(response => response.Name, "Name", "Name"),
                new EntityField<BankResponse>(response => response.RoutingNumber, "Routing Number", "RoutingNumber"),
                new EntityField<BankResponse>(response => response.SwiftCode, "SWIFT Code", "SwiftCode"),
                new EntityField<BankResponse>(response => response.ContactPerson, "Contact Person", "ContactPerson"),
                new EntityField<BankResponse>(response => response.PhoneNumber, "Phone", "PhoneNumber"),
                new EntityField<BankResponse>(response => response.Email, "Email", "Email"),
                new EntityField<BankResponse>(response => response.IsActive, "Status", "IsActive", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<BankSearchCommand>();
                var result = await Client.BankSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<BankResponse>>();
            },
            createFunc: async viewModel =>
            {
                viewModel.Image = new FileUploadCommand
                {
                    Name = viewModel.Image?.Name,
                    Extension = viewModel.Image?.Extension,
                    Data = viewModel.Image?.Data,
                    Size = viewModel.Image?.Size,
                };
                await Client.BankCreateEndpointAsync("1", viewModel.Adapt<BankCreateCommand>());
            },
            updateFunc: async (id, viewModel) =>
            {
                viewModel.Image = new FileUploadCommand
                {
                    Name = viewModel.Image?.Name,
                    Extension = viewModel.Image?.Extension,
                    Data = viewModel.Image?.Data,
                    Size = viewModel.Image?.Size,
                };
                await Client.BankUpdateEndpointAsync("1", id, viewModel.Adapt<BankUpdateCommand>());
            },
            deleteFunc: async id => await Client.BankDeleteEndpointAsync("1", id));
}


