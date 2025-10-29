using FSH.Starter.Blazor.Client.Services;

namespace FSH.Starter.Blazor.Client.Pages.Store.Suppliers;

/// <summary>
/// Suppliers page logic. Provides CRUD and server-side search over Supplier entities via the generated API client.
/// Mirrors Budgets/Items patterns for consistent UX.
/// </summary>
public partial class Suppliers
{
    [Inject] protected ImageUrlService ImageUrlService { get; set; } = default!;

    protected EntityServerTableContext<SupplierResponse, DefaultIdType, SupplierViewModel> Context { get; set; } = default!;
    private EntityTable<SupplierResponse, DefaultIdType, SupplierViewModel> _table = default!;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<SupplierResponse, DefaultIdType, SupplierViewModel>(
            entityName: "Supplier",
            entityNamePlural: "Suppliers",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<SupplierResponse>(response => response.ImageUrl, "Image", "ImageUrl", Template: TemplateImage),
                new EntityField<SupplierResponse>(response => response.Code, "Code", "Code"),
                new EntityField<SupplierResponse>(response => response.Name, "Name", "Name"),
                new EntityField<SupplierResponse>(response => response.ContactPerson, "Contact", "ContactPerson"),
                new EntityField<SupplierResponse>(response => response.Email, "Email", "Email"),
                new EntityField<SupplierResponse>(response => response.Phone, "Phone", "Phone"),
                new EntityField<SupplierResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<SupplierResponse>(response => response.Rating, "Rating", "Rating", typeof(decimal))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id ?? DefaultIdType.Empty,
            // getDetailsFunc: async id =>
            // {
            //     var dto = await Client.GetSupplierEndpointAsync("1", id).ConfigureAwait(false);
            //     return dto.Adapt<SupplierViewModel>();
            // }
            searchFunc: async filter =>
            {
                var command = filter.Adapt<SearchSuppliersCommand>();
                var result = await Client.SearchSuppliersEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<SupplierResponse>>();
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
                await Client.CreateSupplierEndpointAsync("1", viewModel.Adapt<CreateSupplierCommand>()).ConfigureAwait(false);
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
                await Client.UpdateSupplierEndpointAsync("1", id, viewModel.Adapt<UpdateSupplierCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteSupplierEndpointAsync("1", id).ConfigureAwait(false));
    }
}

