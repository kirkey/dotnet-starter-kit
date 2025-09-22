namespace FSH.Starter.Blazor.Client.Pages.Accounting.Accruals;

/// <summary>
/// Accruals page logic. Provides CRUD and search over Accrual entities using the generated API client.
/// Mirrors patterns used by other Accounting pages (Budgets, ChartOfAccounts).
/// </summary>
public partial class Accruals
{
    [Inject] protected IClient ApiClient { get; set; } = default!;

    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<AccrualResponse, DefaultIdType, AccrualViewModel> Context { get; set; } = default!;

    private EntityTable<AccrualResponse, DefaultIdType, AccrualViewModel> _table = default!;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<AccrualResponse, DefaultIdType, AccrualViewModel>(
            entityName: "Accrual",
            entityNamePlural: "Accruals",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<AccrualResponse>(response => response.AccrualNumber, "Number", "AccrualNumber"),
                new EntityField<AccrualResponse>(response => response.AccrualDate, "Date", "AccrualDate", typeof(DateOnly)),
                new EntityField<AccrualResponse>(response => response.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<AccrualResponse>(response => response.IsReversed, "Reversed", "IsReversed", typeof(bool)),
                new EntityField<AccrualResponse>(response => response.ReversalDate, "Reversal Date", "ReversalDate", typeof(DateOnly?)),
                new EntityField<AccrualResponse>(response => response.Description, "Description", "Description"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var query = filter.Adapt<SearchAccrualsQuery>();
                var result = await ApiClient.AccrualSearchEndpointAsync("1", query).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<AccrualResponse>>();
            },
            createFunc: async viewModel =>
            {
                await ApiClient.AccrualCreateEndpointAsync("1", viewModel.Adapt<CreateAccrualCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                if (viewModel is { IsReversed: true, ReversalDate: not null })
                {
                    await ApiClient.AccrualReverseEndpointAsync("1", id, viewModel.Adapt<ReverseAccrualCommand>()).ConfigureAwait(false);
                }
                else
                {
                    await ApiClient.AccrualUpdateEndpointAsync("1", id, viewModel.Adapt<UpdateAccrualCommand>()).ConfigureAwait(false);
                }
            },
            deleteFunc: async id => await ApiClient.AccrualDeleteEndpointAsync("1", id).ConfigureAwait(false));

        return Task.CompletedTask;
    }
}

