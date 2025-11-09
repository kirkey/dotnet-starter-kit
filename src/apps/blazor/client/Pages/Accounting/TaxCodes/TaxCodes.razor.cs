namespace FSH.Starter.Blazor.Client.Pages.Accounting.TaxCodes;

/// <summary>
/// Tax Codes page logic. Provides CRUD and search over TaxCode entities using the generated API client.
/// Manages tax rate configuration for sales tax, VAT, GST, and other tax calculations.
/// </summary>
public partial class TaxCodes
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<TaxCodeResponse, DefaultIdType, TaxCodeViewModel> Context { get; set; } = default!;

    private EntityTable<TaxCodeResponse, DefaultIdType, TaxCodeViewModel> _table = default!;

    /// <summary>
    /// Initializes the table context with tax code-specific configuration including fields, CRUD operations, and search functionality.
    /// </summary>
    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<TaxCodeResponse, DefaultIdType, TaxCodeViewModel>(
            fields:
            [
                new EntityField<TaxCodeResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<TaxCodeResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<TaxCodeResponse>(dto => dto.TaxType, "Tax Type", "TaxType"),
                new EntityField<TaxCodeResponse>(dto => dto.Rate * 100m, "Rate %", "Rate"),
                new EntityField<TaxCodeResponse>(dto => dto.Jurisdiction, "Jurisdiction", "Jurisdiction"),
                new EntityField<TaxCodeResponse>(dto => dto.IsCompound, "Compound", "IsCompound", typeof(bool)),
                new EntityField<TaxCodeResponse>(dto => dto.EffectiveDate, "Effective Date", "EffectiveDate", typeof(DateOnly)),
                new EntityField<TaxCodeResponse>(dto => dto.ExpiryDate, "Expiry Date", "ExpiryDate", typeof(DateOnly?)),
                new EntityField<TaxCodeResponse>(dto => dto.TaxAuthority, "Tax Authority", "TaxAuthority"),
                new EntityField<TaxCodeResponse>(dto => dto.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchTaxCodesCommand();
                var result = await Client.TaxCodeSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<TaxCodeResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                var command = viewModel.Adapt<CreateTaxCodeCommand>();
                // Convert percentage to decimal (e.g., 8.25 -> 0.0825)
                command.Rate = viewModel.Rate / 100m;
                await Client.TaxCodeCreateEndpointAsync("1", command);
            },
            updateFunc: async (id, viewModel) =>
            {
                var command = new UpdateTaxCodeCommand
                {
                    Id = id,
                    Name = viewModel.Name,
                    Jurisdiction = viewModel.Jurisdiction,
                    TaxAuthority = viewModel.TaxAuthority,
                    TaxRegistrationNumber = viewModel.TaxRegistrationNumber,
                    ReportingCategory = viewModel.ReportingCategory,
                    Description = viewModel.Description
                };
                await Client.TaxCodeUpdateEndpointAsync("1", id, command);
            },
            deleteFunc: async id => await Client.TaxCodeDeleteEndpointAsync("1", id),
            // getDetailsFunc: async id =>
            // {
            //     var details = await Client.TaxCodeGetEndpointAsync("1", id);
            //     var viewModel = details.Adapt<TaxCodeViewModel>();
            //     // Convert decimal to percentage for display (e.g., 0.0825 -> 8.25)
            //     viewModel.Rate = details.Rate * 100m;
            //     return viewModel;
            // },
            entityName: "Tax Code",
            entityNamePlural: "Tax Codes",
            entityResource: FshResources.Accounting);
}

