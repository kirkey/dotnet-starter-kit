namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payees;

/// <summary>
/// Payees page logic. Provides CRUD and search over Payee entities using the generated API client.
/// Mirrors patterns used by other Accounting pages (Accruals, Budgets, ChartOfAccounts).
/// </summary>
public partial class Payees
{
    [Inject] protected ImageUrlService ImageUrlService { get; set; } = null!;

    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<PayeeResponse, DefaultIdType, PayeeViewModel> Context { get; set; } = null!;

    private EntityTable<PayeeResponse, DefaultIdType, PayeeViewModel> _table = null!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<PayeeResponse, DefaultIdType, PayeeViewModel>(
            entityName: "Payee",
            entityNamePlural: "Payees",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<PayeeResponse>(response => response.ImageUrl, "Picture", "ImageUrl", Template: TemplateImage),
                new EntityField<PayeeResponse>(response => response.PayeeCode, "Payee Code", "PayeeCode"),
                new EntityField<PayeeResponse>(response => response.ExpenseAccountCode, "Account Code", "ExpenseAccountCode"),
                new EntityField<PayeeResponse>(response => response.ExpenseAccountName, "Account Name", "ExpenseAccountName"),
                new EntityField<PayeeResponse>(response => response.Name, "Name", "Name"),
                new EntityField<PayeeResponse>(response => response.Address, "Address", "Address"),
                new EntityField<PayeeResponse>(response => response.Tin, "TIN", "Tin"),
                new EntityField<PayeeResponse>(response => response.Description, "Description", "Description"),
                new EntityField<PayeeResponse>(response => response.Notes, "Notes", "Notes"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new PayeeSearchCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.PayeeSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<PayeeResponse>>();
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
                await Client.PayeeCreateEndpointAsync("1", viewModel.Adapt<PayeeCreateCommand>());
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
                await Client.PayeeUpdateEndpointAsync("1", id, viewModel.Adapt<PayeeUpdateCommand>());
            },
            deleteFunc: async id => await Client.PayeeDeleteEndpointAsync("1", id));

    /// <summary>
    /// Show payees help dialog.
    /// </summary>
    private async Task ShowPayeesHelp()
    {
        await DialogService.ShowAsync<PayeesHelpDialog>("Payees Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
