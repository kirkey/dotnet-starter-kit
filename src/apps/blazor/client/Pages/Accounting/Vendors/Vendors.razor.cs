namespace FSH.Starter.Blazor.Client.Pages.Accounting.Vendors;

/// <summary>
/// Vendors page logic. Provides CRUD and search over Vendor entities using the generated API client.
/// Manages vendor/supplier information including billing details, contact information, and default expense account mappings.
/// </summary>
public partial class Vendors
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<VendorSearchResponse, DefaultIdType, VendorViewModel> Context { get; set; } = default!;

    private EntityTable<VendorSearchResponse, DefaultIdType, VendorViewModel> _table = default!;

    /// <summary>
    /// Initializes the table context with vendor-specific configuration including fields, CRUD operations, and search functionality.
    /// </summary>
    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<VendorSearchResponse, DefaultIdType, VendorViewModel>(
            entityName: "Vendor",
            entityNamePlural: "Vendors",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<VendorSearchResponse>(response => response.VendorCode, "Vendor Code", "VendorCode"),
                new EntityField<VendorSearchResponse>(response => response.Name, "Name", "Name"),
                new EntityField<VendorSearchResponse>(response => response.Address, "Address", "Address"),
                new EntityField<VendorSearchResponse>(response => response.ExpenseAccountCode, "Account Code", "ExpenseAccountCode"),
                new EntityField<VendorSearchResponse>(response => response.ExpenseAccountName, "Account Name", "ExpenseAccountName"),
                new EntityField<VendorSearchResponse>(response => response.Tin, "TIN", "Tin"),
                new EntityField<VendorSearchResponse>(response => response.Phone, "Phone", "Phone"),
                new EntityField<VendorSearchResponse>(response => response.Description, "Description", "Description"),
                new EntityField<VendorSearchResponse>(response => response.Notes, "Notes", "Notes"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<VendorSearchQuery>();
                var result = await Client.VendorSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<VendorSearchResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.VendorCreateEndpointAsync("1", viewModel.Adapt<VendorCreateCommand>());
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.VendorUpdateEndpointAsync("1", id, viewModel.Adapt<VendorUpdateCommand>());
            },
            deleteFunc: async id => await Client.VendorDeleteEndpointAsync("1", id));
}

