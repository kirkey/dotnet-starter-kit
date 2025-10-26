namespace FSH.Starter.Blazor.Client.Pages.Store.Suppliers;

/// <summary>
/// Suppliers page logic. Provides CRUD and server-side search over Supplier entities via the generated API client.
/// Mirrors Budgets/Items patterns for consistent UX.
/// </summary>
public partial class Suppliers
{
    

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
                new EntityField<SupplierResponse>(x => x.Code, "Code", "Code"),
                new EntityField<SupplierResponse>(x => x.Name, "Name", "Name"),
                new EntityField<SupplierResponse>(x => x.ContactPerson, "Contact", "ContactPerson"),
                new EntityField<SupplierResponse>(x => x.Email, "Email", "Email"),
                new EntityField<SupplierResponse>(x => x.Phone, "Phone", "Phone"),
                new EntityField<SupplierResponse>(x => x.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<SupplierResponse>(x => x.Rating, "Rating", "Rating", typeof(decimal))
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
                await Client.CreateSupplierEndpointAsync("1", viewModel.Adapt<CreateSupplierCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateSupplierEndpointAsync("1", id, viewModel.Adapt<UpdateSupplierCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteSupplierEndpointAsync("1", id).ConfigureAwait(false));
    }
}

/// <summary>
/// ViewModel for Supplier add/edit operations.
/// Inherits from UpdateSupplierCommand to ensure proper mapping with the API.
/// Includes contact info, address, payment terms, status, and notes.
/// </summary>
public partial class SupplierViewModel : UpdateSupplierCommand;
