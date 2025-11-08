namespace FSH.Starter.Blazor.Client.Pages.Accounting.Vendors;

/// <summary>
/// Vendors page for managing vendor accounts and supplier information.
/// </summary>
public partial class Vendors
{
    /// <summary>
    /// The entity table context for managing vendors.
    /// </summary>
    protected EntityServerTableContext<VendorSearchResponse, DefaultIdType, VendorViewModel> Context { get; set; } = default!;

    /// <summary>
    /// Reference to the EntityTable component.
    /// </summary>
    private EntityTable<VendorSearchResponse, DefaultIdType, VendorViewModel> _table = default!;

    // Search filters
    private string? VendorCode { get; set; }
    private string? VendorName { get; set; }
    private string? Phone { get; set; }

    /// <summary>
    /// Initializes the component and sets up the entity table context.
    /// </summary>
    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<VendorSearchResponse, DefaultIdType, VendorViewModel>(
            entityName: "Vendor",
            entityNamePlural: "Vendors",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<VendorSearchResponse>(vendor => vendor.VendorCode, "Code", "VendorCode"),
                new EntityField<VendorSearchResponse>(vendor => vendor.Name, "Name", "Name"),
                new EntityField<VendorSearchResponse>(vendor => vendor.Address, "Address", "Address"),
                new EntityField<VendorSearchResponse>(vendor => vendor.ContactPerson, "Contact Person", "ContactPerson"),
                new EntityField<VendorSearchResponse>(vendor => vendor.Phone, "Phone", "Phone"),
                new EntityField<VendorSearchResponse>(vendor => vendor.Email, "Email", "Email"),
                new EntityField<VendorSearchResponse>(vendor => vendor.ExpenseAccountCode, "Expense Account", "ExpenseAccountCode"),
                new EntityField<VendorSearchResponse>(vendor => vendor.Tin, "TIN", "Tin"),
            ],
            enableAdvancedSearch: true,
            idFunc: vendor => vendor.Id,
            searchFunc: async filter =>
            {
                var searchQuery = new VendorSearchQuery
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };

                var result = await Client.VendorSearchEndpointAsync("1", searchQuery);
                return result.Adapt<PaginationResponse<VendorSearchResponse>>();
            },
            // getDetailsFunc: async id =>
            // {
            //     var vendor = await Client.VendorGetEndpointAsync("1", id);
            //     return vendor.Adapt<VendorViewModel>();
            // },
            createFunc: async viewModel =>
            {
                var command = new VendorCreateCommand
                {
                    VendorCode = viewModel.VendorCode,
                    Name = viewModel.Name,
                    Address = viewModel.Address,
                    BillingAddress = viewModel.BillingAddress,
                    ContactPerson = viewModel.ContactPerson,
                    Email = viewModel.Email,
                    Terms = viewModel.Terms,
                    ExpenseAccountCode = viewModel.ExpenseAccountCode,
                    ExpenseAccountName = viewModel.ExpenseAccountName,
                    Tin = viewModel.Tin,
                    Phone = viewModel.Phone,
                    Description = viewModel.Description,
                    Notes = viewModel.Notes
                };

                await Client.VendorCreateEndpointAsync("1", command);
                Snackbar.Add($"Vendor {viewModel.Name} created successfully", Severity.Success);
            },
            updateFunc: async (id, viewModel) =>
            {
                var command = new VendorUpdateCommand
                {
                    Id = id,
                    VendorCode = viewModel.VendorCode,
                    Name = viewModel.Name,
                    Address = viewModel.Address,
                    BillingAddress = viewModel.BillingAddress,
                    ContactPerson = viewModel.ContactPerson,
                    Email = viewModel.Email,
                    Terms = viewModel.Terms,
                    ExpenseAccountCode = viewModel.ExpenseAccountCode,
                    ExpenseAccountName = viewModel.ExpenseAccountName,
                    Tin = viewModel.Tin,
                    Phone = viewModel.Phone,
                    Description = viewModel.Description,
                    Notes = viewModel.Notes
                };

                await Client.VendorUpdateEndpointAsync("1", id, command);
                Snackbar.Add($"Vendor {viewModel.Name} updated successfully", Severity.Success);
            },
            deleteFunc: async id =>
            {
                await Client.VendorDeleteEndpointAsync("1", id);
                Snackbar.Add("Vendor deleted successfully", Severity.Success);
            });

        base.OnInitialized();
    }
}

