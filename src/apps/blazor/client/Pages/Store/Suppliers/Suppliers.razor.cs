namespace FSH.Starter.Blazor.Client.Pages.Store.Suppliers;

/// <summary>
/// Suppliers page logic. Provides CRUD and server-side search over Supplier entities via the generated API client.
/// Mirrors Budgets/Items patterns for consistent UX. Supports workflow actions for activation/deactivation.
/// </summary>
public partial class Suppliers
{
    [Inject] protected ImageUrlService ImageUrlService { get; set; } = null!;
    

    protected EntityServerTableContext<SupplierResponse, DefaultIdType, SupplierViewModel> Context { get; set; } = null!;

    private ClientPreference _preference = new();
    private EntityTable<SupplierResponse, DefaultIdType, SupplierViewModel> _table = null!;

    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

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
            searchFunc: async filter =>
            {
                var command = new SearchSuppliersCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchSuppliersEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<SupplierResponse>>();
            },
            createFunc: async viewModel =>
            {
                viewModel.Code = viewModel.Code?.ToUpperInvariant() ?? viewModel.Code;
                viewModel.Name = viewModel.Name?.ToUpperInvariant() ?? viewModel.Name;
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
                viewModel.Code = viewModel.Code?.ToUpperInvariant() ?? viewModel.Code;
                viewModel.Name = viewModel.Name?.ToUpperInvariant() ?? viewModel.Name;
                viewModel.Image = new FileUploadCommand
                {
                    Name = viewModel.Image?.Name,
                    Extension = viewModel.Image?.Extension,
                    Data = viewModel.Image?.Data,
                    Size = viewModel.Image?.Size,
                };
                await Client.UpdateSupplierEndpointAsync("1", id, viewModel.Adapt<UpdateSupplierCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteSupplierEndpointAsync("1", id).ConfigureAwait(false),
            hasExtraActionsFunc: () => true);
    }

    /// <summary>
    /// Navigate to supplier dashboard.
    /// </summary>
    private void OnViewDashboard(DefaultIdType id)
    {
        Navigation.NavigateTo($"/store/suppliers/{id}/dashboard");
    }

    /// <summary>
    /// Activates a deactivated supplier.
    /// </summary>
    private async Task ActivateSupplier(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Activation",
            "Are you sure you want to activate this supplier?",
            yesText: "Activate",
            cancelText: "Cancel");

        if (result == true)
        {
            try
            {
                var command = new ActivateSupplierCommand();
                await Client.ActivateSupplierEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Supplier activated successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to activate supplier: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Deactivates an active supplier.
    /// </summary>
    private async Task DeactivateSupplier(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Deactivation",
            "Are you sure you want to deactivate this supplier? Active purchase orders may be affected.",
            yesText: "Deactivate",
            cancelText: "Cancel");

        if (result == true)
        {
            try
            {
                var command = new DeactivateSupplierCommand();
                await Client.DeactivateSupplierEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Supplier deactivated successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to deactivate supplier: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Show suppliers help dialog.
    /// </summary>
    private async Task ShowSuppliersHelp()
    {
        await DialogService.ShowAsync<SuppliersHelpDialog>("Suppliers Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

