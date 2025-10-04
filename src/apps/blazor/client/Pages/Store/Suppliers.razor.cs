namespace FSH.Starter.Blazor.Client.Pages.Store;

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
                new EntityField<SupplierResponse>(x => x.City, "City", "City"),
                new EntityField<SupplierResponse>(x => x.Country, "Country", "Country"),
                new EntityField<SupplierResponse>(x => x.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<SupplierResponse>(x => x.Rating, "Rating", "Rating", typeof(decimal))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id ?? DefaultIdType.Empty,
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
            deleteFunc: async id => await Client.DeleteSupplierEndpointAsync("1", id).ConfigureAwait(false),
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetSupplierEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<SupplierViewModel>();
            });
    }
}

/// <summary>
/// ViewModel for Supplier add/edit operations. Maps to CreateSupplierCommand for creation and UpdateSupplierCommand for updates.
/// Includes contact info, address, payment terms, status, and notes.
/// </summary>
public class SupplierViewModel
{
    /// <summary>Unique supplier identifier.</summary>
    public DefaultIdType Id { get; set; }

    /// <summary>Supplier display name. Required; max length 200.</summary>
    public string? Name { get; set; }

    /// <summary>Optional description up to 2000 chars.</summary>
    public string? Description { get; set; }

    /// <summary>Unique supplier code. Required; max length 50.</summary>
    public string? Code { get; set; }

    /// <summary>Primary contact person. Required; max length 100.</summary>
    public string? ContactPerson { get; set; }

    /// <summary>Contact email. Required; max length 255.</summary>
    public string? Email { get; set; }

    /// <summary>Contact phone. Required; max length 50.</summary>
    public string? Phone { get; set; }

    /// <summary>Street address. Required; max length 500.</summary>
    public string? Address { get; set; }

    /// <summary>City. Required; max length 100.</summary>
    public string? City { get; set; }

    /// <summary>State or region. Optional; max length 100.</summary>
    public string? State { get; set; }

    /// <summary>Country. Required; max length 100.</summary>
    public string? Country { get; set; }

    /// <summary>Postal or ZIP code. Optional; max length 20.</summary>
    public string? PostalCode { get; set; }

    /// <summary>Website URL. Optional; max length 255.</summary>
    public string? Website { get; set; }

    /// <summary>Optional credit limit; must be >= 0 when provided.</summary>
    public decimal? CreditLimit { get; set; }

    /// <summary>Payment terms in days; default 30; must be >= 0.</summary>
    public int PaymentTermsDays { get; set; } = 30;

    /// <summary>Activation status; default true.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Rating between 0 and 5; default 0.</summary>
    public decimal Rating { get; set; }

    /// <summary>Optional notes up to 2000 chars.</summary>
    public string? Notes { get; set; }
}
