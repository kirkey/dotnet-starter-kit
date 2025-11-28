namespace FSH.Starter.Blazor.Client.Pages.Hr.Benefits;

/// <summary>
/// Benefits page for managing employee benefits per Philippine Labor Code.
/// Provides CRUD operations for benefit definitions.
/// </summary>
public partial class Benefits
{
    protected EntityServerTableContext<BenefitAllocationResponse, DefaultIdType, BenefitViewModel> Context { get; set; } = null!;

    private EntityTable<BenefitAllocationResponse, DefaultIdType, BenefitViewModel>? _table;

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<BenefitAllocationResponse, DefaultIdType, BenefitViewModel>(
            entityName: "Benefit",
            entityNamePlural: "Benefits",
            entityResource: FshResources.Benefits,
            fields:
            [
                new EntityField<BenefitAllocationResponse>(response => response.EmployeeName, "Employee", "EmployeeName"),
                new EntityField<BenefitAllocationResponse>(response => response.BenefitName, "Benefit", "BenefitName"),
                new EntityField<BenefitAllocationResponse>(response => response.AllocatedAmount, "Amount", "AllocatedAmount", typeof(decimal)),
                new EntityField<BenefitAllocationResponse>(response => response.AllocationDate, "Date", "AllocationDate", typeof(DateTime)),
                new EntityField<BenefitAllocationResponse>(response => response.Status, "Status", "Status"),
                new EntityField<BenefitAllocationResponse>(response => response.AllocationType ?? "-", "Type", "AllocationType"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchBenefitAllocationsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchBenefitAllocationsEndpointAsync("1", request).ConfigureAwait(false);

                return result.Adapt<PaginationResponse<BenefitAllocationResponse>>();
            },
            createFunc: async benefit =>
            {
                await Client.CreateBenefitAllocationEndpointAsync("1", benefit.Adapt<CreateBenefitAllocationCommand>()).ConfigureAwait(false);
            });

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Shows the benefits help dialog.
    /// </summary>
    private async Task ShowBenefitsHelp()
    {
        await DialogService.ShowAsync<BenefitsHelpDialog>("Benefits Help", new DialogParameters(), _helpDialogOptions);
    }
}
